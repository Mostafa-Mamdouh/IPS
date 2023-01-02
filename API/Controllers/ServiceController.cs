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
    public class ServiceController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ServiceController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<ActionResult<Pagination<ServiceToReturnDto>>> GetServices(
  [FromQuery] InventorySpecParams inventorySpecParams)
        {
            var spec = new ServiceListSpecification(inventorySpecParams);
            var countSpec = new ServiceListCountSpecification(inventorySpecParams);
            var totalItems = await _unitOfWork.Repository<Service>().CountAsync(countSpec);
            var services = await _unitOfWork.Repository<Service>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ServiceToReturnDto>>(services);
            return Ok(new Pagination<ServiceToReturnDto>(inventorySpecParams.PageIndex,
                inventorySpecParams.PageSize, totalItems, data));
        }
        [HttpGet("lookup")]
        public async Task<ActionResult<IReadOnlyList<ServiceToReturnDto>>> GetServicesLookup(
[FromQuery] InventorySpecParams inventorySpecParams)
        {
            var spec = new ServiceListSpecification(inventorySpecParams);
            var services = await _unitOfWork.Repository<Service>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ServiceToReturnDto>>(services);
            return Ok(data);
        }

        [HttpGet("export")]
        public async Task<IActionResult> ExportServices(
[FromQuery] InventorySpecParams inventorySpecParams)
        {

            var spec = new ServiceListSpecification(inventorySpecParams);
            var services = await _unitOfWork.Repository<Service>().ListAsync(spec);

            var data = services.Select(x => new
            {
                ServiceID = x.Id,
                ServiceName = x.Name,
                CategoryName = x.Category.Name,
                Description = x.Description,
                CreatedBy=x.CreateUser.FirstName + " " + x.CreateUser.LastName,
                CreationDate=x.CreateDate
            });
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("ServiceList_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
                workSheet.Cells["A1:F1"].Style.Font.Bold = true;
                workSheet.Cells["A1:F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1:F1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(112, 48, 160));
                workSheet.Cells["A1:F1"].Style.Font.Color.SetColor(Color.White);
                var Count = data.Count() + 2;
                workSheet.Cells["F2:F" + Count + ""].Style.Numberformat.Format = "dd-mmm-yyyy";
                workSheet.Cells.AutoFitColumns(15);
                workSheet.Cells[1, 1].LoadFromCollection(data, true);

                await package.SaveAsync();
            }
            stream.Position = 0;
            string excelName = $"ServiceList_" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ServiceToReturnDto>> GetService(int id)
        {
            var spec = new ServiceListSpecification(id);
            var service = await _unitOfWork.Repository<Service>().GetEntityWithSpec(spec);
            if (service == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<ServiceToReturnDto>(service);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ServiceToReturnDto>> CreateService(ServiceDto serviceDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var service = _mapper.Map<Service>(serviceDto);
            _unitOfWork.Repository<Service>().Add(service, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating service"));
            return Ok(_mapper.Map<ServiceToReturnDto>(service));
        }
        [HttpGet("Serviceexists")]
        public async Task<ActionResult<bool>> CheckServiceExistsAsync([FromQuery]  int categoryId,int id, string name)
        {
            var spec = new ServiceListSpecification(categoryId, id,name);
            return await _unitOfWork.Repository<Service>().GetEntityWithSpec(spec) != null;
        }


        [HttpPost("update")]
        public async Task<ActionResult<ServiceToReturnDto>> UpdateService(ServiceDto serviceDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var service = await _unitOfWork.Repository<Service>().GetByIdAsync(serviceDto.Id);
            var editableService = _mapper.Map<ServiceDto, Service>(serviceDto, service);
            _unitOfWork.Repository<Service>().Update(editableService, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating service"));
            return Ok(_mapper.Map<ServiceToReturnDto>(editableService));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ServiceToReturnDto>> DeleteService(int id)
        {
            var purchaseSpec = new PurchaseInvoiceProductsByServiceIdSpecification(id);
            var purchaseInvoiceServices = await _unitOfWork.Repository<PurchaseInvoiceProduct>().GetEntityWithSpec(purchaseSpec);
            var SalesSpec = new SalesInvoiceProductsByServiceIdSpecification(id);
            var salesInvoiceServices = await _unitOfWork.Repository<SalesInvoiceProduct>().GetEntityWithSpec(SalesSpec);
            if (purchaseInvoiceServices!=null || salesInvoiceServices!=null)
            {
                return BadRequest(new ApiResponse(400, "Cannot Delete Servive as it is associated with invoices"));
            }
            var service = await _unitOfWork.Repository<Service>().GetByIdAsync(id);
            _unitOfWork.Repository<Service>().Delete(service);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting service"));
            return Ok(_mapper.Map<ServiceToReturnDto>(service));
        }
    }
}
