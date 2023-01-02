using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class CreditController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreditController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("update")]
        public async Task<ActionResult<CreditDto>> UpdateCredit(CreditDto  creditDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var credit = await _unitOfWork.Repository<Credit>().GetByIdAsync(creditDto.Id);
            var editableCredit = _mapper.Map<CreditDto, Credit>(creditDto, credit);
            editableCredit.Year = DateTime.Now.Year;
            _unitOfWork.Repository<Credit>().Update(editableCredit, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating Credit"));
            return Ok(_mapper.Map<CreditDto>(editableCredit));
        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreditDto>> GetCredit(int id)
        {
            var credit = await _unitOfWork.Repository<Credit>().GetByIdAsync(id);
            if (credit == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<CreditDto>(credit);
        }
    }
}
