using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace API.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UsersController : BaseApiController
    {
        private readonly IConfiguration _config;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMailService _mailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor,
         ITokenService tokenService, IMapper mapper, IUnitOfWork unitOfWork, IMailService mailService, IConfiguration config)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mailService = mailService;
            _config = config;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<UserDto>>> GetUsers(
      [FromQuery] UserSpecParams userParams)
        {
            return await Task.Run(() =>
             {
                 var totalItems = _userManager.Users.Count();
                 var users = _userManager.Users.Where(u => (string.IsNullOrEmpty(userParams.Search) ||
                u.FirstName.ToLower().Contains(userParams.Search) || u.LastName.ToLower().Contains(userParams.Search) ||
                u.Email.ToLower().Contains(userParams.Search))).Skip(userParams.PageIndex - 1).Take(userParams.PageSize);
                 var data = _mapper.Map<IReadOnlyList<UserDto>>(users);
                 return Ok(new Pagination<UserDto>(userParams.PageIndex,
                    userParams.PageSize, totalItems, data));
             });
        }
        [HttpGet("export")]
        public async Task<ActionResult<Pagination<UserDto>>> ExportGetUsers(
 [FromQuery] UserSpecParams userParams)
        {

            var totalItems = _userManager.Users.Count();
            var users = _userManager.Users;
            var data = _mapper.Map<IReadOnlyList<UserDto>>(users);

            var export = data.Select(x => new
            {
                UserId = x.UserId,
                DisplayName = x.DisplayName,
                Email = x.Email,
                Status = x.IsActive && x.EmailConfirmed ? "Active" : x.IsActive && !x.EmailConfirmed ? "Pending" : "Not Active",
            });
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("UserList_" + DateTime.Now.ToString("dd MMM yyyy") + "");
                workSheet.Cells.Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells.Style.Fill.BackgroundColor.SetColor(Color.White);
                workSheet.Cells.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                workSheet.Cells.Style.Border.Left.Color.SetColor(Color.Black);
                workSheet.Cells.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                workSheet.Cells.Style.Border.Right.Color.SetColor(Color.Black);
                workSheet.Cells.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                workSheet.Cells.Style.Border.Top.Color.SetColor(Color.Black);
                workSheet.Cells.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                workSheet.Cells.Style.Border.Bottom.Color.SetColor(Color.Black);
                workSheet.Cells["A1:D1"].Style.Font.Bold = true;
                workSheet.Cells["A1:D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:D1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                workSheet.Cells["A1:D1"].Style.Font.Color.SetColor(Color.White);
                var Count = export.Count() + 2;
                workSheet.Cells.AutoFitColumns(15);
                workSheet.Cells[1, 1].LoadFromCollection(export, true);

                await package.SaveAsync();
            }
            stream.Position = 0;
            string excelName = $"UserList_" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientToReturnDto>> GetUser(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound(new ApiResponse(404));
            var returnedUser = _mapper.Map<UserDto>(user);
            var userClaims = await _userManager.GetClaimsAsync(user);
            returnedUser.Claims = _mapper.Map<List<ClaimsDto>>(userClaims);
            var createdByUser= await  _userManager.FindByIdAsync(user.CreateUserId.ToString());
            returnedUser.CreatedBy = createdByUser.FirstName + " " + createdByUser.LastName;
            return Ok(returnedUser);
        }
        [HttpGet("activate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientToReturnDto>> ActivateUser(int id, bool isActive)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null) return NotFound(new ApiResponse(404));
            var userId = int.Parse(User.FindFirstValue("UserId"));
            user.IsActive = !isActive;
            user.UpdateDate = DateTime.Now;
            user.UpdateUserId = userId;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var returnedUser = _mapper.Map<UserDto>(user);
                return Ok(returnedUser);
            }
            else
            {
                return BadRequest(new ApiResponse(400, "Problem on activating user"));
            }
        }
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email,int id)
        {
            if (id==0)
            {
                return await _userManager.FindByEmailAsync(email) != null;

            }
            else
            {
                return _userManager.Users.Any(x => x.Email == email && x.Id != id);
            }

        }

        [HttpPost("create")]
        public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var user = _mapper.Map<AppUser>(userDto);
            var userPassword = Encryption.GenerateRandomPassword();
            user.CreateDate = DateTime.Now;
            user.UpdateDate = DateTime.Now;
            user.CreateUserId = userId;
            user.UpdateUserId = userId;
            user.EmailConfirmed = false;
            user.UserName = (userDto.FirstName + userDto.LastName).Trim();
            var result = await _userManager.CreateAsync(user, userPassword);
            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var userRoles = userDto.Claims.Select(x => x.Type).Distinct();
                await _userManager.AddToRolesAsync(user, userRoles);
                await _userManager.AddClaimsAsync(user, _mapper.Map<List<Claim>>(userDto.Claims));
                var returnedUser = _mapper.Map<UserDto>(user);
                MailRequest request = new MailRequest();

                var callBackUrl = string.Format("{0}{1}", _config["ClientSettings:URL"], "/users/change-password/" + WebUtility.UrlEncode(token) + "/" + user.Id + "/" + false + "");
                request.Subject = "IPS - Email Verification Request";
                request.Body = ""
                + "Email: " + user.Email + " " + "<br />"
                + "Password: " + userPassword + " " + "<br />"
                + "We Received a Request to verify this Email for your IPS Account. Please Click the link below to verify your Email " + "<br />"
                 + callBackUrl + "";
                request.ToEmail = user.Email;
                await _mailService.SendEmailAsync(request);
                return Ok(returnedUser);
            }

            return BadRequest(new ApiResponse(400, "Problem on creating user"));

        }

        [AllowAnonymous]
        [HttpPost("change-password")]
        public async Task<ActionResult<UserDto>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(changePasswordDto.id.ToString());
            user.EmailConfirmed = true;
            IdentityResult result=null;
            if (changePasswordDto.OldPassword!="")
            {
                result = await _userManager.ChangePasswordAsync(user, changePasswordDto.OldPassword, changePasswordDto.NewPassword);
            }
            else
            {
                result = await _userManager.ResetPasswordAsync(user, changePasswordDto.Token, changePasswordDto.NewPassword);
            }

            if (result.Succeeded)
            {
               var updatedResult= await _userManager.UpdateAsync(user);
                if (updatedResult.Succeeded)
                {
                    var returnedUser = _mapper.Map<UserDto>(user);
                    return Ok(returnedUser);
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "Problem on change user password"));
                }
            }
            return BadRequest(new ApiResponse(400, "Problem on change user password"));
        }

        [AllowAnonymous]
        [HttpPost("forget-password")]
        public async Task<ActionResult<UserDto>> ForgetPassword(ResetPasswordDto resetPasswordDto)
        {
            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user!=null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var returnedUser = _mapper.Map<UserDto>(user);
                MailRequest request = new MailRequest();

                var callBackUrl = string.Format("{0}{1}", _config["ClientSettings:URL"], "/users/change-password/" + WebUtility.UrlEncode(token)  + "/"+user.Id+"/"+true+"");
                request.Subject = "IPS - Reset Password Request";
                request.Body = ""
                + "Email: " + user.Email + " " + "<br />"
                + "We Received a Request to Change your Password for your IPS Account. Please Click the link below to change your password " + "<br />"
                + callBackUrl + "";
                request.ToEmail = user.Email;
                await _mailService.SendEmailAsync(request);
                return Ok(returnedUser);
            }
            return BadRequest(new ApiResponse(400, "This email not exist"));
        }

        [HttpPost("update")]
        public async Task<ActionResult<UserDto>> UpdateUser(UserDto userDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var user = await _userManager.FindByIdAsync(userDto.UserId.ToString());
            var editableUser = _mapper.Map<UserDto, AppUser>(userDto, user);
            editableUser.UpdateDate = DateTime.Now;
            editableUser.UpdateUserId = userId;
            editableUser.UserName = editableUser.FirstName + editableUser.LastName;

            var result = await _userManager.UpdateAsync(editableUser);
            if (result.Succeeded)
            {
                var userClaimsInDB = await _userManager.GetClaimsAsync(editableUser);
                var userRolesInDB = await _userManager.GetRolesAsync(editableUser);

                var userRoles = userDto.Claims.Select(x => x.Type).Distinct();
                var userClaims = userDto.Claims;


                var deletedRoles = userRolesInDB.Where(p => userRoles.All(p2 => p2 != p)).ToList();
                var AddedRoles = userRoles.Where(p => userRolesInDB.All(p2 => p2 != p)).ToList();
                var deletedClaims = userClaimsInDB.Where(p => userClaims.All(p2 => p2.Value != p.Value && p2.Type != p.Type)).ToList();
                var AddedClaims = userClaims.Where(p => userClaimsInDB.All(p2 => p2.Value != p.Value && p2.Type != p.Type)).ToList();


                await _userManager.AddToRolesAsync(user, AddedRoles);
                await _userManager.RemoveFromRolesAsync(user, deletedRoles);
                await _userManager.AddClaimsAsync(user, _mapper.Map<List<Claim>>(AddedClaims));
                await _userManager.RemoveClaimsAsync(user, _mapper.Map<List<Claim>>(deletedClaims));


                return Ok(editableUser);
            }
            return BadRequest(new ApiResponse(400, "Problem on updating user"));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<UserDto>> DeleteUser( int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            var userClaimsInDB = await _userManager.GetClaimsAsync(user);
            var userRolesInDB = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveClaimsAsync(user, userClaimsInDB);
            await _userManager.RemoveFromRolesAsync(user, userRolesInDB);
            var result =await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Ok(user);
            }
            return BadRequest(new ApiResponse(400, "Problem on updating user"));
        }



    }
}
