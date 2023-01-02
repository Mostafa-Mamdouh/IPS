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

namespace API.Controllers
{

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ClientController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ClientController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ClientToReturnDto>>> GetClients(
          [FromQuery] ClientSpecParams clientParams)
        {
            var spec = new ClientListSpecification(clientParams);
            var countSpec = new ClientListCountSpecification(clientParams);
            var totalItems = await _unitOfWork.Repository<Client>().CountAsync(countSpec);
            var clients = await _unitOfWork.Repository<Client>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ClientToReturnDto>>(clients);
            return Ok(new Pagination<ClientToReturnDto>(clientParams.PageIndex,
                clientParams.PageSize, totalItems, data));
        }
        [HttpGet("lookup")]
        public async Task<ActionResult<IReadOnlyList<ClientToReturnDto>>> GetClientsLookup(
     [FromQuery] ClientSpecParams clientParams)
        {
            var spec = new ClientListSpecification(clientParams);
            var clients = await _unitOfWork.Repository<Client>().ListAsync(spec);
            var data = _mapper.Map<IReadOnlyList<ClientToReturnDto>>(clients);
            return Ok(data);
        }
        [HttpGet("export")]
        public async Task<IActionResult> ExportClients(
[FromQuery] ClientSpecParams clientParams)
        {

            var spec = new ClientListSpecification(clientParams);
            var clients = await _unitOfWork.Repository<Client>().ListAsync(spec);

            var data = clients.Select(x => new
            {
                ClientID = x.Id,
                CompanyName = x.Name,
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
                var workSheet = package.Workbook.Worksheets.Add("ClientList_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
            string excelName = $"ClientList_" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ClientToReturnDto>> GetClient(int id)
        {
            var spec = new ClientListSpecification(id);
            var client = await _unitOfWork.Repository<Client>().GetEntityWithSpec(spec);
            if (client == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<ClientToReturnDto>(client);
        }

        [HttpGet("clientexists")]
        public async Task<ActionResult<bool>> CheckClientExistsAsync([FromQuery] string name, int id, string taxRefNo)
        {
            var spec = new ClientListSpecification(name, id, taxRefNo);
            return await _unitOfWork.Repository<Client>().GetEntityWithSpec(spec) != null;
        }

        [HttpPost("create")]
        public async Task<ActionResult<ClientToReturnDto>> CreateClient(ClientDto clientDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var client = _mapper.Map<Client>(clientDto);
            _unitOfWork.Repository<Client>().Add(client, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating client"));
            return Ok(_mapper.Map<ClientToReturnDto>(client));
        }


        [HttpPost("update")]
        public async Task<ActionResult<ClientToReturnDto>> UpdateClient(ClientDto clientDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var client = await _unitOfWork.Repository<Client>().GetByIdAsync(clientDto.Id);
            var editableClient = _mapper.Map<ClientDto, Client>(clientDto, client);
            _unitOfWork.Repository<Client>().Update(editableClient, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating client"));
            return Ok(_mapper.Map<ClientToReturnDto>(editableClient));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ClientToReturnDto>> DeleteClient(int id)
        {
            var salesSpec = new SalesInvoiceByClientIdSpecification(id);
            var salesInvoices = await _unitOfWork.Repository<SalesInvoice>().GetEntityWithSpec(salesSpec);
            if (salesInvoices != null)
            {
                return BadRequest(new ApiResponse(400, "Cannot Delete client as it is associated with sales invoices"));
            }

            var client = await _unitOfWork.Repository<Client>().GetByIdAsync(id);
            _unitOfWork.Repository<Client>().Delete(client);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting client"));
            return Ok(_mapper.Map<ClientToReturnDto>(client));
        }


    }
}
