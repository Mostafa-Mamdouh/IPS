using API.Dtos;
using API.Errors;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SalesController : BaseApiController
    {
        #region Invoice
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public SalesController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<InvoiceToReturnDto>>> GetInvoices(
          [FromQuery] SalesSpecParams salesParams)
        {
            var spec = new SalesListSpecification(salesParams);
            var countSpec = new SalesListCountSpecification(salesParams);
            var totalItems = await _unitOfWork.Repository<SalesInvoice>().CountAsync(countSpec);
            var invoices = await _unitOfWork.Repository<SalesInvoice>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<InvoiceToReturnDto>>(invoices);
            return Ok(new Pagination<InvoiceToReturnDto>(salesParams.PageIndex,
                salesParams.PageSize, totalItems, data));
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportInvoices(
[FromQuery] SalesSpecParams salesParams)
        {
            var spec = new SalesListSpecification(salesParams);
            var Invoices = await _unitOfWork.Repository<SalesInvoice>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<InvoiceToReturnDto>>(Invoices);
            var report = data.Select(x => new
            {
                ID = x.Id,
                ClientName = x.ClientName,
                InvoiceDate = x.InvoiceDate.ToString("dd/MM/yyyy"),
                InvoiceNumber = x.SalesInvoiceNumber,
                TotalInvoice = x.TotalInvoice,
                TotalPaid = x.TotalPaid,
                Status = x.Deleted ? "Cancelled" : "Active",
                Type = x.IsTax ? "ضريبية" : "غير ضريبية"
            });
            var Count = report.Count() + 2;
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sales-Invoice-List_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
                workSheet.Cells["A1:H1"].Style.Font.Bold = true;
                workSheet.Cells["A1:H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:H1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                workSheet.Cells["A1:H1"].Style.Font.Color.SetColor(Color.White);

                workSheet.Cells["E2:E" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["F2:F" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["C2:C" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";
                workSheet.Cells.AutoFitColumns(15);

                workSheet.Cells[1, 1].LoadFromCollection(report, true);

                await package.SaveAsync();
            }
            stream.Position = 0;


            string excelName = $"Sales-Invoice-List" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

        }

        [HttpGet("report")]
        public async Task<IActionResult> GenerateInvoiceReport(
[FromQuery] SalesSpecParams salesParams)
        {
            string StoredProc = @"Exec dbo.Sp_SalesInvoiceReport @InvoiceFromDate,@InvoiceToDate";
            var parameter = new List<SqlParameter>
            {
                SqlHelper.CreateSqlParameter("@InvoiceFromDate", SqlDbType.DateTime, salesParams.InvoiceFromDate),
                SqlHelper.CreateSqlParameter("@InvoiceToDate", SqlDbType.DateTime, salesParams.InvoiceToDate),
            };
            var report = await _unitOfWork.Repository<Sp_SalesInvoiceReport>().StoredProc(StoredProc, parameter.ToArray());
            var Count = report.Count() + 2;
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Sales-Invoice-Report_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
                workSheet.Cells.AutoFitColumns(25);
                workSheet.Cells["A1:Y1"].Style.Font.Bold = true;
                workSheet.Cells["A1:Y1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:Y1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                workSheet.Cells["A1:Y1"].Style.Font.Color.SetColor(Color.White);

                workSheet.Cells["J2:J" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["K2:K" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["L2:L" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["M2:M" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["N2:N" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["O2:O" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["P2:P" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["Q2:Q" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["R2:R" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["V2:V" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["W2:W" + Count + ""].Style.Numberformat.Format = "#,##0.000";



                workSheet.Cells["S2:S" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";
                workSheet.Cells["D2:D" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";


                workSheet.Cells.AutoFitColumns(15);
                workSheet.Cells[1, 1].LoadFromCollection(report, true);

                workSheet.DeleteColumn(26);
                workSheet.DeleteColumn(26);
                workSheet.DeleteColumn(26);
                workSheet.DeleteColumn(26);

                await package.SaveAsync();
            }
            stream.Position = 0;


            string excelName = $"Sales-Invoice-Report" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

        }
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InvoiceToReturnDto>> GetInvoice(int id)
        {
            var spec = new SalesListSpecification(id);
            var invoice = await _unitOfWork.Repository<SalesInvoice>().GetEntityWithSpec(spec);
            if (invoice == null) return NotFound(new ApiResponse(404));
            var mappedInvoice = _mapper.Map<InvoiceToReturnDto>(invoice);

            var invoiceProductSpec = new SalesInvoiceProductsSpecification(mappedInvoice.Id);
            var invoiceProducts = await _unitOfWork.Repository<SalesInvoiceProduct>().ListAsync(invoiceProductSpec);
            var mappedInvoiceProducts = _mapper.Map<ICollection<InvoiceProductToReturnDto>>(invoiceProducts);
            mappedInvoice.InvoiceProducts = mappedInvoiceProducts;

            var invoicePaymentSpec = new SalesInvoicePaymentsSpecification(mappedInvoice.Id);
            var invoicePayments = await _unitOfWork.Repository<SalesInvoicePayment>().ListAsync(invoicePaymentSpec);
            var mappedInvoicePayments = _mapper.Map<ICollection<InvoicePaymentToReturnDto>>(invoicePayments);
            mappedInvoice.InvoicePayments = mappedInvoicePayments;

            var CreateUser = await _userManager.FindByIdAsync(mappedInvoice.CreateUserId.ToString());
            mappedInvoice.CreatedBy = CreateUser.FirstName + " " + CreateUser.LastName;
            return Ok(mappedInvoice);
        }

        [HttpPost("create")]
        public async Task<ActionResult<InvoiceToReturnDto>> CreateInvoice(SalesInvoiceDto salesInvoiceDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            if (salesInvoiceDto.IsTax)
            {
                string StoredProc = @"Exec dbo.SP_GetSalesInvoiceNumber";
                var salesInvoiceNumber = await _unitOfWork.Repository<SP_GetSalesInvoiceNumber>().StoredProc(StoredProc);
                salesInvoiceDto.SalesInvoiceNumber =salesInvoiceNumber.Any()? salesInvoiceNumber.FirstOrDefault().SalesInvoiceNumber:1;
            }

            var salesInvoice = _mapper.Map<SalesInvoice>(salesInvoiceDto);
            _unitOfWork.Repository<SalesInvoice>().Add(salesInvoice, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating sales invoice"));
            return Ok(_mapper.Map<InvoiceToReturnDto>(salesInvoice));
        }


        [HttpPost("update")]
        public async Task<ActionResult<InvoiceToReturnDto>> UpdateInvoice(SalesInvoiceDto salesInvoiceDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var salesInvoice = await _unitOfWork.Repository<SalesInvoice>().GetByIdAsync(salesInvoiceDto.Id);
            var editableInvoice = _mapper.Map<SalesInvoiceDto, SalesInvoice>(salesInvoiceDto, salesInvoice);

            foreach (var item in salesInvoiceDto.deletedIds)
            {
                var salesInvoiceProduct = await _unitOfWork.Repository<SalesInvoiceProduct>().GetByIdAsync(item);
                _unitOfWork.Repository<SalesInvoiceProduct>().Delete(salesInvoiceProduct);
            }

            _unitOfWork.Repository<SalesInvoice>().Update(editableInvoice, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating sales invoice"));
            return Ok(_mapper.Map<InvoiceToReturnDto>(editableInvoice));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<InvoiceToReturnDto>> DeleteInvoice(int id)
        {
           
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var salesInvoice = await _unitOfWork.Repository<SalesInvoice>().GetByIdAsync(id);
            _unitOfWork.Repository<SalesInvoice>().Delete(salesInvoice, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting sales invoice"));
            return Ok(_mapper.Map<InvoiceToReturnDto>(salesInvoice));
        }

        #endregion

        #region Payment
        [HttpPost("create-payment")]
        public async Task<ActionResult<InvoicePaymentToReturnDto>> CreatePayment(InvoicePaymentDto invoicePaymentDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var salesInvoicePayment = _mapper.Map<SalesInvoicePayment>(invoicePaymentDto);
            _unitOfWork.Repository<SalesInvoicePayment>().Add(salesInvoicePayment, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating Sales invoice payment"));
            return Ok(_mapper.Map<InvoicePaymentToReturnDto>(salesInvoicePayment));
        }

        [HttpPost("update-payment")]
        public async Task<ActionResult<InvoicePaymentToReturnDto>> UpdatePayment(InvoicePaymentDto invoicePaymentDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var salesInvoicePayment = await _unitOfWork.Repository<SalesInvoicePayment>().GetByIdAsync(invoicePaymentDto.Id);
            var editableInvoicePayment = _mapper.Map<InvoicePaymentDto, SalesInvoicePayment>(invoicePaymentDto, salesInvoicePayment);
            _unitOfWork.Repository<SalesInvoicePayment>().Update(editableInvoicePayment, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating sales invoice payment"));
            return Ok(_mapper.Map<InvoicePaymentToReturnDto>(editableInvoicePayment));
        }

        [HttpGet("payment/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InvoicePaymentToReturnDto>> GetPayment(int id)
        {
            var spec = new SalesPaymentSpecification(id);
            var invoicePayment = await _unitOfWork.Repository<SalesInvoicePayment>().GetEntityWithSpec(spec);
            if (invoicePayment == null) return NotFound(new ApiResponse(404));
            var mappedInvoicePayment = _mapper.Map<InvoicePaymentToReturnDto>(invoicePayment);
            var CreateUser = await _userManager.FindByIdAsync(mappedInvoicePayment.CreateUserId.ToString());
            mappedInvoicePayment.CreatedBy = CreateUser.FirstName + " " + CreateUser.LastName;
            return Ok(mappedInvoicePayment);
        }
        [HttpGet("invoice-payment/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IReadOnlyList<InvoicePaymentToReturnDto>>> GetPaymentByInvoiceId(int invoiceId)
        {
            var spec = new SalesPaymentSpecification(invoiceId,true);
            var invoicePayments = await _unitOfWork.Repository<SalesInvoicePayment>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<InvoicePaymentToReturnDto>>(invoicePayments);
            if (data.Any())
            {
                var CreateUser = await _userManager.FindByIdAsync(data.FirstOrDefault().CreateUserId.ToString());
                foreach (var item in data)
                {
                    item.CreatedBy= CreateUser.FirstName + " " + CreateUser.LastName;
                }
                
            }

            return Ok(data);
        }

        [HttpDelete("payment/{id}")]
        public async Task<ActionResult<InvoicePaymentToReturnDto>> DeletePayment(int id)
        {

            var salesInvoicePayment = await _unitOfWork.Repository<SalesInvoicePayment>().GetByIdAsync(id);
            _unitOfWork.Repository<SalesInvoicePayment>().Delete(salesInvoicePayment);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting sales invoice payment"));
            return Ok(_mapper.Map<InvoicePaymentToReturnDto>(salesInvoicePayment));
        }


        #endregion
    }
}
