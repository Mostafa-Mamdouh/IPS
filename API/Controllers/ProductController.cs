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
using System.Collections.Generic;
using System.Security.Claims;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.IO;
using OfficeOpenXml;
using System;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Linq;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class ProductController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<ActionResult<Sp_ProductSearch>> GetProducts(
          [FromQuery] InventorySpecParams productParams)
        {
            string StoredProc = @"Exec dbo.Sp_ProductSearch @SearchKeyword,@CreateFromDate,@CreateToDate,@CategoryID,@Page,@Size,@SortBy,@SortDirection";
            var parameter = new SqlParameter[]
            {
                SqlHelper.CreateSqlParameter("@SearchKeyword", SqlDbType.NVarChar, productParams.Search),
                SqlHelper.CreateSqlParameter("@CreateFromDate", SqlDbType.DateTime, productParams.CreateFromDate),
                SqlHelper.CreateSqlParameter("@CreateToDate", SqlDbType.NVarChar, productParams.CreateToDate),
                SqlHelper.CreateSqlParameter("@CategoryID", SqlDbType.Int, productParams.CategoryId),
                SqlHelper.CreateSqlParameter("@Page", SqlDbType.Int, productParams.PageIndex),
                SqlHelper.CreateSqlParameter("@Size", SqlDbType.Int, productParams.PageSize),
                SqlHelper.CreateSqlParameter("@SortBy", SqlDbType.NVarChar, productParams.Sort),
                SqlHelper.CreateSqlParameter("@SortDirection", SqlDbType.NVarChar, productParams.SortDirection)
            };
            var products = await _unitOfWork.Repository<Sp_ProductSearch>().StoredProc(StoredProc, parameter);
            return Ok(new Pagination<Sp_ProductSearch>(productParams.PageIndex,
                productParams.PageSize, products.Any()?products.FirstOrDefault().TotalCount:0, products));
        }
        [HttpGet("lookup")]
        public async Task<ActionResult<IReadOnlyList<Sp_ProductSearch>>> GetProductsLookup(
  [FromQuery] InventorySpecParams productParams)
        {
            string StoredProc = @"Exec dbo.Sp_ProductSearch @SearchKeyword,@CreateFromDate,@CreateToDate,@CategoryID,@Page,@Size,@SortBy,@SortDirection";
            var parameter = new SqlParameter[]
            {
                SqlHelper.CreateSqlParameter("@SearchKeyword", SqlDbType.NVarChar, productParams.Search),
                SqlHelper.CreateSqlParameter("@CreateFromDate", SqlDbType.DateTime, productParams.CreateFromDate),
                SqlHelper.CreateSqlParameter("@CreateToDate", SqlDbType.NVarChar, productParams.CreateToDate),
                SqlHelper.CreateSqlParameter("@CategoryID", SqlDbType.Int, productParams.CategoryId),
                SqlHelper.CreateSqlParameter("@Page", SqlDbType.Int, productParams.PageIndex),
                SqlHelper.CreateSqlParameter("@Size", SqlDbType.Int, productParams.PageSize),
                SqlHelper.CreateSqlParameter("@SortBy", SqlDbType.Int, productParams.Sort),
                SqlHelper.CreateSqlParameter("@SortDirection", SqlDbType.Int, productParams.SortDirection)
            };
            var products = await _unitOfWork.Repository<Sp_ProductSearch>().StoredProc(StoredProc, parameter);
            return Ok(products);
        }
        [HttpGet("export")]
        public async Task<IActionResult> ExportProducts(
   [FromQuery] InventorySpecParams productParams)
        {
            string StoredProc = @"Exec dbo.Sp_ProductSearch @SearchKeyword,@CreateFromDate,@CreateToDate,@CategoryID,@Page,@Size,@SortBy,@SortDirection";
            var parameter = new List<SqlParameter>
            {
                SqlHelper.CreateSqlParameter("@SearchKeyword", SqlDbType.NVarChar, productParams.Search),
                SqlHelper.CreateSqlParameter("@CreateFromDate", SqlDbType.DateTime, productParams.CreateFromDate),
                SqlHelper.CreateSqlParameter("@CreateToDate", SqlDbType.NVarChar, productParams.CreateToDate),
                SqlHelper.CreateSqlParameter("@CategoryID", SqlDbType.Int, productParams.CategoryId),
                SqlHelper.CreateSqlParameter("@Page", SqlDbType.Int, productParams.PageIndex),
                SqlHelper.CreateSqlParameter("@Size", SqlDbType.Int, productParams.PageSize),
                SqlHelper.CreateSqlParameter("@SortBy", SqlDbType.Int, productParams.Sort),
                SqlHelper.CreateSqlParameter("@SortDirection", SqlDbType.Int, productParams.SortDirection)
            };
            var products = await _unitOfWork.Repository<Sp_ProductSearch>().StoredProc(StoredProc, parameter.ToArray());

            var data = products.Select(x => new
            {
                ProductID = x.Id,
                ProductName = x.ProductName,
                Code = x.Code,
                CategoryName = x.CategoryName,
                Description = x.Description,
                Inbound = x.Inbound,
                Outbound = x.Outbound,
                Stock = x.Stock,
                CreatedBy=x.CreatedBy,
                CreationDate=x.CreateDate
            });
            var stream = new MemoryStream();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage(stream))
            {
                var workSheet = package.Workbook.Worksheets.Add("ProductList_" + DateTime.Now.ToString("dd MMM yyyy") + "");
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
            string excelName = $"ProductList_" + DateTime.Now.ToString("dd MMM yyyy") + ".xlsx";
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var spec = new ProductListSpecification(id);
            var product = await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec);
            if (product == null) return NotFound(new ApiResponse(404));
            return _mapper.Map<ProductToReturnDto>(product);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ProductToReturnDto>> CreateProduct(ProductDto productDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var product = _mapper.Map<Product>(productDto);
            _unitOfWork.Repository<Product>().Add(product, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on creating product"));
            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }

        [HttpGet("productexists")]
        public async Task<ActionResult<bool>> CheckProductExistsAsync([FromQuery] int categoryId,string code,int id)
        {
            var spec = new ProductListSpecification(categoryId, code,id);
            return  await _unitOfWork.Repository<Product>().GetEntityWithSpec(spec) != null;
        }


        [HttpPost("update")]
        public async Task<ActionResult<ProductToReturnDto>> UpdateProduct(ProductDto productDto)
        {
            var userId = int.Parse(User.FindFirstValue("UserId"));
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(productDto.Id);
            var editableProduct = _mapper.Map<ProductDto, Product>(productDto, product);
            _unitOfWork.Repository<Product>().Update(editableProduct, userId);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on updating product"));
            return Ok(_mapper.Map<ProductToReturnDto>(editableProduct));
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> DeleteProduct(int id)
        {
            var purchaseSpec = new PurchaseInvoiceProductsByProductIdSpecification(id);
            var purchaseInvoiceProducts = await _unitOfWork.Repository<PurchaseInvoiceProduct>().GetEntityWithSpec(purchaseSpec);
            var SalesSpec = new SalesInvoiceProductsByProductIdSpecification(id);
            var salesInvoiceProducts = await _unitOfWork.Repository<SalesInvoiceProduct>().GetEntityWithSpec(SalesSpec);
            if (purchaseInvoiceProducts != null || salesInvoiceProducts != null)
            {
                return BadRequest(new ApiResponse(400, "Cannot Delete Product as it is associated with invoices"));
            }
            var product = await _unitOfWork.Repository<Product>().GetByIdAsync(id);
            _unitOfWork.Repository<Product>().Delete(product);
            var result = await _unitOfWork.Complete();
            if (result <= 0) return BadRequest(new ApiResponse(400, "Problem on deleting product"));
            return Ok(_mapper.Map<ProductToReturnDto>(product));
        }
    }
}
