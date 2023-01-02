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
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class SupplierController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SupplierController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<SupplierToReturnDto>>> GetSuppliers(
  [FromQuery] SupplierSpecParams supplierParams)
        {
            var spec = new SupplierListSpecification(supplierParams);
            var countSpec = new SupplierListCountSpecification(supplierParams);
            var totalItems = await _unitOfWork.Repository<Supplier>().CountAsync(countSpec);
            var suppliers = await _unitOfWork.Repository<Supplier>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<SupplierToReturnDto>>(suppliers);
            return Ok(new Pagination<SupplierToReturnDto>(supplierParams.PageIndex,
                supplierParams.PageSize, totalItems, data));
        }
        [HttpGet("lookup")]
        public async Task<ActionResult<IReadOnlyList<SupplierToReturnDto>>> GetSuppliersLookup(
  [FromQuery] SupplierSpecParams supplierParams)
        {
            var spec = new SupplierListSpecification(supplierParams);
            var suppliers = await _unitOfWork.Repository<Supplier>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<SupplierToReturnDto>>(suppliers);
            return Ok(data);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportSuppliers(
[FromQuery] SupplierSpecParams supplierParams)
        {

            var spec = new SupplierListSpecification(supplierParams);
            var suppliers = await _unitOfWork.Repository<Supplier>().ListAsync(spec);

            var data = suppliers.Select(x => new
            {
                SupplierID = x.Id,
                SupplierName = x.Name,
                city = x.City.Name,
                Address = x.Address,
                TaxReferenceNumber = x.TaxReferenceNumber,
                RepresentativeName = x.RepresentativeName,
                Email = x.Email,
                MobileNumber = x.MobileNumber,
                CreatedBy = x.CreateUser.FirstName + " " + x.CreateUser.LastName,
                CreationDate = x.CreateDate
            });
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("SupplierList_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
                workSheet.Cells["A1:J1"].Style.Font.Bold = true;
                workSheet.Cells["A1:J1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:J1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                workSheet.Cells["A1:J1"].Style.Font.Color.SetColor(Color.White);
                var Count = data.Count() + 2;
                workSheet.Cells["J2:J" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";
                workSheet.Cells.AutoFitColumns(15);
                workSheet.Cells[1, 1].LoadFromCollection(data, true);

                await package.SaveAsync();
            }
            stream.Position = 0;
            string excelName = $"SupplierList_" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SupplierToReturnDto>> GetSupplier(int id)
        {
            var spec = new SupplierListSpecification(id);
            var supplier = await _unitOfWork.Repository<Supplier>().GetEntityWithSpec(spec);
            if (supplier == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<SupplierToReturnDto>(supplier);
        }

        [HttpGet("supplierexists")]
        public async Task<ActionResult<bool>> CheckProductExistsAsync([FromQuery] string name, int id, string taxRefNo)
        {
            var spec = new SupplierListSpecification(name,id,taxRefNo);
            return await _unitOfWork.Repository<Supplier>().GetEntityWithSpec(spec) != null;
        }

        [HttpPost("create")]
        public async Task<ActionResult<SupplierToReturnDto>> CreateSupplier(SupplierDto supplierDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var supplier = _mapper.Map<Supplier>(supplierDto);
            _unitOfWork.Repository<Supplier>().Add(supplier, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating supplier"));
            return Ok(_mapper.Map<SupplierToReturnDto>(supplier));
        }


        [HttpPost("update")]
        public async Task<ActionResult<SupplierToReturnDto>> UpdateSupplier(SupplierDto supplierDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(supplierDto.Id);
            var editableSupplier = _mapper.Map<SupplierDto, Supplier>(supplierDto, supplier);
            _unitOfWork.Repository<Supplier>().Update(editableSupplier, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating supplier"));
            return Ok(_mapper.Map<SupplierToReturnDto>(editableSupplier));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<SupplierToReturnDto>> DeleteSupplier(int id)
        {
            var purchaseSpec = new PurchaseInvoiceBySupplierIdSpecification(id);
            var purchaseInvoices = await _unitOfWork.Repository<PurchaseInvoice>().GetEntityWithSpec(purchaseSpec);
            if (purchaseInvoices != null)
            {
                return BadRequest(new ApiResponse(400, "Cannot Delete supplier as it is associated with purchase invoices"));
            }
            var supplier = await _unitOfWork.Repository<Supplier>().GetByIdAsync(id);
            _unitOfWork.Repository<Supplier>().Delete(supplier);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting supplier"));
            return Ok(_mapper.Map<SupplierToReturnDto>(supplier));
        }
    }
}
