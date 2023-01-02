using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]


    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountController> _logger;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper, ILogger<AccountController> logger)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrinciple(User);
            if (user == null) return new UserDto();

            var userClaims = await _userManager.GetClaimsAsync(user);
            return new UserDto
            {
                UserId = user.Id,
                IsAuthenticated = true,
                IsActive = user.IsActive,
                EmailConfirmed = user.EmailConfirmed,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.FirstName + " " + user.LastName,
                Claims = (List<ClaimsDto>)_mapper.Map<IList<ClaimsDto>>(userClaims)
            };
        }
       

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            var userClaims =await _userManager.GetClaimsAsync(user);
            
            return new UserDto
            {
                UserId = user.Id,
                IsAuthenticated=true,
                IsActive=user.IsActive,
                EmailConfirmed=user.EmailConfirmed,
                FirstName=user.FirstName,
                LastName=user.LastName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.FirstName + " " + user.LastName,
                Claims = (List<ClaimsDto>)_mapper.Map<IList<ClaimsDto>>(userClaims)
            };
        }


    }
}
