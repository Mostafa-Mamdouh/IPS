using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static API.Helpers.EnumData;

namespace API.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ExpenseController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ExpenseController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ExpenseToReturnDto>>> GetExpenses(
          [FromQuery] ExpenseSpecParams ExpenseParams)
        {
            var spec = new ExpenseListSpecification(ExpenseParams);
            var countSpec = new ExpenseListCountSpecification(ExpenseParams);
            var totalItems = await _unitOfWork.Repository<Expense>().CountAsync(countSpec);
            var Expenses = await _unitOfWork.Repository<Expense>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ExpenseToReturnDto>>(Expenses);
            return Ok(new Pagination<ExpenseToReturnDto>(ExpenseParams.PageIndex,
                ExpenseParams.PageSize, totalItems, data));
        }
   
        [HttpGet("export")]
        public async Task<IActionResult> ExportExpenses(
[FromQuery] ExpenseSpecParams ExpenseParams)
        {

            var spec = new ExpenseListSpecification(ExpenseParams);
            var Expenses = await _unitOfWork.Repository<Expense>().ListAsync(spec);

            var data = Expenses.Select(x => new
            {
                ExpenseID = x.Id,
                Type = x.ExpenseType.Name,
                Description = x.Description,
                TransctionDate = x.TransactionDate,
                Amount = x.Amount,
                CreatedBy = x.CreateUser.FirstName + " " + x.CreateUser.LastName,
                CreationDate = x.CreateDate
            });
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("ExpensesList_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
                workSheet.Cells["A1:G1"].Style.Font.Bold = true;
                workSheet.Cells["A1:G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:G1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                workSheet.Cells["A1:G1"].Style.Font.Color.SetColor(Color.White);
                var Count = data.Count() + 2;
                workSheet.Cells["D2:D" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";
                workSheet.Cells["G2:G" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";

                workSheet.Cells.AutoFitColumns(15);
                workSheet.Cells[1, 1].LoadFromCollection(data, true);

                await package.SaveAsync();
            }
            stream.Position = 0;
            string excelName = $"ExpensesList_" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ExpenseToReturnDto>> GetExpense(int id)
        {
            var spec = new ExpenseListSpecification(id);
            var Expense = await _unitOfWork.Repository<Expense>().GetEntityWithSpec(spec);
            if (Expense == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<ExpenseToReturnDto>(Expense);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ExpenseToReturnDto>> CreateExpense(ExpenseDto ExpenseDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var Expense = _mapper.Map<Expense>(ExpenseDto);
            _unitOfWork.Repository<Expense>().Add(Expense, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating Expense"));
            return Ok(_mapper.Map<ExpenseToReturnDto>(Expense));
        }


        [HttpPost("update")]
        public async Task<ActionResult<ExpenseToReturnDto>> UpdateExpense(ExpenseDto ExpenseDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var Expense = await _unitOfWork.Repository<Expense>().GetByIdAsync(ExpenseDto.Id);
            var editableExpense = _mapper.Map<ExpenseDto, Expense>(ExpenseDto, Expense);
            _unitOfWork.Repository<Expense>().Update(editableExpense, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating Expense"));
            return Ok(_mapper.Map<ExpenseToReturnDto>(editableExpense));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ExpenseToReturnDto>> DeleteExpense(int id)
        {
            var Expense = await _unitOfWork.Repository<Expense>().GetByIdAsync(id);
            _unitOfWork.Repository<Expense>().Delete(Expense);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting Expense"));
            return Ok(_mapper.Map<ExpenseToReturnDto>(Expense));
        }


    }
}
