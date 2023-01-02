using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class UploadController : BaseApiController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UploadController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }

        [HttpGet("download")]
        public async Task<IActionResult> Download([FromQuery] string file)
        {
            var folderPath = Path.Combine(Directory.GetCurrentDirectory());
            var filePath = Path.Combine(folderPath, file);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(filePath), file);
        }
        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadAsync()
        {
            var formCollection = await Request.ReadFormAsync();
            var file = formCollection.Files.First();
            var folderName = Path.Combine("Resources");
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            var pathToSave ="";
            string YearMonth = "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString() + "/";
            if (Directory.Exists(folderPath + "/" + DateTime.Now.Year.ToString()))
            {
                if (!Directory.Exists(folderPath + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString()))
                {
                    Directory.CreateDirectory(folderPath + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString());
                }
            }
            else
            {
                Directory.CreateDirectory(folderPath + "/" + DateTime.Now.Year.ToString());
                Directory.CreateDirectory(folderPath + YearMonth);
            }
            pathToSave = folderPath + "/" + DateTime.Now.Year.ToString() + "/" + DateTime.Now.Month.ToString();
            if (file.Length > 0)
            {

                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                FileInfo fi = new FileInfo(fileName);
                if(fi.Extension.ToLower() != ".jpg" && fi.Extension.ToLower() != ".jpeg" && fi.Extension.ToLower() != ".png"  && fi.Extension.ToLower() != ".xlsx" && fi.Extension != ".pdf"&& fi.Extension.ToLower() != ".doc" && fi.Extension.ToLower() != ".docs" && fi.Extension.ToLower() != ".xls")
                {
                    return BadRequest(new ApiResponse(400, "File format is not valid"));
                }
               var NewFileName = fileName.Replace(fi.Extension, String.Format("_attach_" + "{0:yyyy-MM-dd_hh-mm-ss-tt}", DateTime.Now) + fi.Extension);

                var fullPath = Path.Combine(pathToSave, NewFileName);
                var dbPath = Path.Combine(folderName + YearMonth, NewFileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                var systemArchieve = new SystemArchive()
                {
                    FilePath = dbPath,
                    FileName = NewFileName
                };
                var userId = int.Parse(User.FindFirstValue("UserId"));
                _unitOfWork.Repository<SystemArchive>().Add(systemArchieve, userId);
                var result = await _unitOfWork.Complete();
                if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on uploading file"));
                return Ok(_mapper.Map<AttachmentDto>(systemArchieve));

            }
            else
            {
                return BadRequest(new ApiResponse(400, "Problem on uploading file"));
            }

        }

    }
}
