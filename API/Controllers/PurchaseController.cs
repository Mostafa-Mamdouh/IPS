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
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class PurchaseController : BaseApiController
    {
        #region Invoice
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public PurchaseController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<InvoiceToReturnDto>>> GetInvoices(
          [FromQuery] PurchaseSpecParams purchaseParams)
        {
            var spec = new PurchaseListSpecification(purchaseParams);
            var countSpec = new PurchaseListCountSpecification(purchaseParams);
            var totalItems = await _unitOfWork.Repository<PurchaseInvoice>().CountAsync(countSpec);
            var Invoices = await _unitOfWork.Repository<PurchaseInvoice>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<InvoiceToReturnDto>>(Invoices);
            return Ok(new Pagination<InvoiceToReturnDto>(purchaseParams.PageIndex,
                purchaseParams.PageSize, totalItems, data));
        }
        [HttpGet("export")]
        public async Task<IActionResult> ExportInvoices(
    [FromQuery] PurchaseSpecParams purchaseParams)
        {
            var spec = new PurchaseListSpecification(purchaseParams);
            var Invoices = await _unitOfWork.Repository<PurchaseInvoice>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<InvoiceToReturnDto>>(Invoices);
            var report = data.Select(x => new
            {
                ID = x.Id,
                SupplierName = x.SupplierName,
                InvoiceDate = x.InvoiceDate.ToString("dd/MM/yyyy"),
                InvoiceNumber = x.PurchaseInvoiceNumber,
                TotalInvoice = x.TotalInvoice,
                TotalPaid = x.TotalPaid,
                Status = x.Deleted?"Cancelled":"Active",
                Type=x.IsTax?"ضريبية":"غير ضريبية"
            });
            var Count = report.Count()+2;
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Purchasing-Invoice-List_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
     

            string excelName = $"Purchasing-Invoice-List" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

        }

        [HttpGet("report")]
        public async Task<IActionResult> GenerateInvoiceReport(
[FromQuery] PurchaseSpecParams purchaseParams)
        {
            string StoredProc = @"Exec dbo.Sp_PurchaseInvoiceReport @InvoiceFromDate,@InvoiceToDate";
            var parameter = new List<SqlParameter>
            {
                SqlHelper.CreateSqlParameter("@InvoiceFromDate", SqlDbType.DateTime, purchaseParams.InvoiceFromDate),
                SqlHelper.CreateSqlParameter("@InvoiceToDate", SqlDbType.DateTime, purchaseParams.InvoiceToDate),
            };
            var report = await _unitOfWork.Repository<Sp_PurchaseInvoiceReport>().StoredProc(StoredProc, parameter.ToArray());
            var Count = report.Count()+2;
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("Purchasing-Invoice-Report_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
                workSheet.Cells["A1:X1"].Style.Font.Bold = true;
                workSheet.Cells["A1:X1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:X1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                workSheet.Cells["A1:X1"].Style.Font.Color.SetColor(Color.White);

                workSheet.Cells["I2:I" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["J2:J" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["K2:K" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["L2:L" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["M2:LM" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["N2:N" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["O2:O" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["P2:P" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["Q2:Q" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["U2:U" + Count + ""].Style.Numberformat.Format = "#,##0.000";
                workSheet.Cells["V2:V" + Count + ""].Style.Numberformat.Format = "#,##0.000";


                workSheet.Cells["R2:R" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";
                workSheet.Cells["D2:D" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";


                workSheet.Cells.AutoFitColumns(15);
                workSheet.Cells[1, 1].LoadFromCollection(report, true);

                workSheet.DeleteColumn(25);
                workSheet.DeleteColumn(25);
                workSheet.DeleteColumn(25);
                workSheet.DeleteColumn(25);

                await package.SaveAsync();
            }
            stream.Position = 0;


            string excelName = $"Purchasing-Invoice-Report" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

        }



        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InvoiceToReturnDto>> GetInvoice(int id)
        {
            var spec = new PurchaseListSpecification(id);
            var invoice = await _unitOfWork.Repository<PurchaseInvoice>().GetEntityWithSpec(spec);
            if (invoice == null) return NotFound(new ApiResponse(404));
            var mappedInvoice = _mapper.Map<InvoiceToReturnDto>(invoice);

            var invoiceProductSpec = new PurchaseInvoiceProductsSpecification(mappedInvoice.Id);
            var invoiceProducts = await _unitOfWork.Repository<PurchaseInvoiceProduct>().ListAsync(invoiceProductSpec);
            var mappedInvoiceProducts = _mapper.Map<ICollection<InvoiceProductToReturnDto>>(invoiceProducts);
            mappedInvoice.InvoiceProducts = mappedInvoiceProducts;

            var invoicePaymentSpec = new PurchaseInvoicePaymentsSpecification(mappedInvoice.Id);
            var invoicePayments = await _unitOfWork.Repository<PurchaseInvoicePayment>().ListAsync(invoicePaymentSpec);
            var mappedInvoicePayments = _mapper.Map<ICollection<InvoicePaymentToReturnDto>>(invoicePayments);
            mappedInvoice.InvoicePayments = mappedInvoicePayments;

            var CreateUser = await _userManager.FindByIdAsync(mappedInvoice.CreateUserId.ToString());
            mappedInvoice.CreatedBy = CreateUser.FirstName + " " + CreateUser.LastName;
            return Ok(mappedInvoice);
        }

        [HttpPost("create")]
        public async Task<ActionResult<InvoiceToReturnDto>> CreateInvoice(PurchaseInvoiceDto purchaseInvoiceDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var purchaseInvoice = _mapper.Map<PurchaseInvoice>(purchaseInvoiceDto);
            _unitOfWork.Repository<PurchaseInvoice>().Add(purchaseInvoice, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating purchase invoice"));
            return Ok(_mapper.Map<InvoiceToReturnDto>(purchaseInvoice));
        }


        [HttpPost("update")]
        public async Task<ActionResult<InvoiceToReturnDto>> UpdateInvoice(PurchaseInvoiceDto purchaseInvoiceDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var purchaseInvoice = await _unitOfWork.Repository<PurchaseInvoice>().GetByIdAsync(purchaseInvoiceDto.Id);
            var editableInvoice = _mapper.Map<PurchaseInvoiceDto, PurchaseInvoice>(purchaseInvoiceDto, purchaseInvoice);

            foreach (var item in purchaseInvoiceDto.deletedIds)
            {
                var purchaseInvoiceProduct = await _unitOfWork.Repository<PurchaseInvoiceProduct>().GetByIdAsync(item);
                _unitOfWork.Repository<PurchaseInvoiceProduct>().Delete(purchaseInvoiceProduct);
            }

            _unitOfWork.Repository<PurchaseInvoice>().Update(editableInvoice, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating purchase invoice"));
            return Ok(_mapper.Map<InvoiceToReturnDto>(editableInvoice));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<InvoiceToReturnDto>> DeleteInvoice(int id)
        {
           
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var spec = new PurchaseListSpecification(id);
            var purchaseInvoice = await _unitOfWork.Repository<PurchaseInvoice>().GetEntityWithSpec(spec);
            foreach (var item in purchaseInvoice.PurchaseInvoicePayments)
            {
                item.Deleted = true;
            }
            foreach (var item in purchaseInvoice.PurchaseInvoiceProducts)
            {
                item.Deleted = true;
            }
            _unitOfWork.Repository<PurchaseInvoice>().Delete(purchaseInvoice, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting purchase invoice"));
            return Ok(_mapper.Map<InvoiceToReturnDto>(purchaseInvoice));
        }

        #endregion


        #region Payment
        [HttpPost("create-payment")]
        public async Task<ActionResult<InvoicePaymentToReturnDto>> CreatePayment(InvoicePaymentDto invoicePaymentDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var purchaseInvoicePayment = _mapper.Map<PurchaseInvoicePayment>(invoicePaymentDto);
            _unitOfWork.Repository<PurchaseInvoicePayment>().Add(purchaseInvoicePayment, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating Sales invoice payment"));
            return Ok(_mapper.Map<InvoicePaymentToReturnDto>(purchaseInvoicePayment));
        }

        [HttpPost("update-payment")]
        public async Task<ActionResult<InvoicePaymentToReturnDto>> UpdatePayment(InvoicePaymentDto invoicePaymentDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var purchaseInvoicePayment = await _unitOfWork.Repository<PurchaseInvoicePayment>().GetByIdAsync(invoicePaymentDto.Id);
            var editableInvoicePayment = _mapper.Map<InvoicePaymentDto, PurchaseInvoicePayment>(invoicePaymentDto, purchaseInvoicePayment);
            _unitOfWork.Repository<PurchaseInvoicePayment>().Update(editableInvoicePayment, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating sales invoice payment"));
            return Ok(_mapper.Map<InvoicePaymentToReturnDto>(editableInvoicePayment));
        }

        [HttpGet("payment/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<InvoicePaymentToReturnDto>> GetPayment(int id)
        {
            var spec = new PurchasePaymentSpecification(id);
            var invoicePayment = await _unitOfWork.Repository<PurchaseInvoicePayment>().GetEntityWithSpec(spec);
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
            var spec = new PurchasePaymentSpecification(invoiceId, true);
            var invoicePayments = await _unitOfWork.Repository<PurchaseInvoicePayment>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<InvoicePaymentToReturnDto>>(invoicePayments);
            if (data.Any())
            {
                var CreateUser = await _userManager.FindByIdAsync(data.FirstOrDefault().CreateUserId.ToString());
                foreach (var item in data)
                {
                    item.CreatedBy = CreateUser.FirstName + " " + CreateUser.LastName;
                }

            }

            return Ok(data);
        }

        [HttpDelete("payment/{id}")]
        public async Task<ActionResult<InvoicePaymentToReturnDto>> DeletePayment(int id)
        {

            var purchaseInvoicePayment = await _unitOfWork.Repository<PurchaseInvoicePayment>().GetByIdAsync(id);
            _unitOfWork.Repository<PurchaseInvoicePayment>().Delete(purchaseInvoicePayment);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting purchase invoice payment"));
            return Ok(_mapper.Map<InvoicePaymentToReturnDto>(purchaseInvoicePayment));
        }


        #endregion
    }
}
