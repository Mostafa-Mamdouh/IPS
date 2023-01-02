using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
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
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class HomeController : BaseApiController
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HomeController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        [HttpGet("country")]
        public async Task<ActionResult<IReadOnlyList<LookupDto>>> GetCountries()
        {
            var countries = await _unitOfWork.Repository<Country>().ListAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<LookupDto>>(countries));
        }

        [HttpGet("cities/{countryId}")]
        public async Task<ActionResult<IReadOnlyList<LookupDto>>> GetCitiesByCountryId(int countryId)
        {
            var spec = new CitySpecification(countryId);
            var cities = await _unitOfWork.Repository<City>().ListAsync(spec);
            return Ok(_mapper.Map<IReadOnlyList<LookupDto>>(cities));
        }

        [HttpGet("category")]
        public async Task<ActionResult<IReadOnlyList<LookupDto>>> GetCategories()
        {
            var categories = await _unitOfWork.Repository<Category>().ListAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<LookupDto>>(categories));
        }

        [HttpGet("expensetypes")]
        public async Task<ActionResult<IReadOnlyList<LookupDto>>> GetExpenseTypes()
        {
            var expenses = await _unitOfWork.Repository<ExpenseType>().ListAllAsync();
            return Ok(_mapper.Map<IReadOnlyList<LookupDto>>(expenses));
        }


        [HttpGet("bank-transaction")]
        public async Task<ActionResult<SP_BankLatestTransactions>> GetBankTransactions(
          [FromQuery] TransactionSpecParams transactionSpecParams)
        {
            string StoredProc = @"Exec dbo.SP_BankLatestTransactions @Page,@Size";
            var parameter = new SqlParameter[]
            {
                SqlHelper.CreateSqlParameter("@Page", SqlDbType.Int, transactionSpecParams.PageIndex),
                SqlHelper.CreateSqlParameter("@Size", SqlDbType.Int, transactionSpecParams.PageSize)
            };
            var bankLatestTransactions = await _unitOfWork.Repository<SP_BankLatestTransactions>().StoredProc(StoredProc, parameter);
            return Ok(new Pagination<SP_BankLatestTransactions>(transactionSpecParams.PageIndex,
                transactionSpecParams.PageSize, bankLatestTransactions.Any()?bankLatestTransactions.FirstOrDefault().Total_Count:0, bankLatestTransactions));
        }


        [HttpGet("cash-transaction")]
        public async Task<ActionResult<SP_CashLatestTransactions>> GetCashTransactions(
         [FromQuery] TransactionSpecParams transactionSpecParams)
        {
            string StoredProc = @"Exec dbo.SP_CashLatestTransactions @Page,@Size";
            var parameter = new SqlParameter[]
            {
                SqlHelper.CreateSqlParameter("@Page", SqlDbType.Int, transactionSpecParams.PageIndex),
                SqlHelper.CreateSqlParameter("@Size", SqlDbType.Int, transactionSpecParams.PageSize)
            };
            var cashLatestTransactions = await _unitOfWork.Repository<SP_CashLatestTransactions>().StoredProc(StoredProc, parameter);
            return Ok(new Pagination<SP_CashLatestTransactions>(transactionSpecParams.PageIndex,
                transactionSpecParams.PageSize, cashLatestTransactions.Any() ? cashLatestTransactions.FirstOrDefault().Total_Count : 0, cashLatestTransactions));
        }
        [AllowAnonymous]
        [HttpGet("dashboard-amounts")]
        public async Task<ActionResult<DashBoardAmountsDto>> GetDashboardAmounts()
        {
            string StoredProc = @"Exec [dbo].[Sp_DashboardAmounts]";
            var dashboardAmounts = await _unitOfWork.Repository<Sp_DashboardAmounts>().StoredProc(StoredProc);
            return _mapper.Map<DashBoardAmountsDto>(dashboardAmounts.FirstOrDefault());
        }

        [AllowAnonymous]
        [HttpGet("line-chart")]
        public async Task<ActionResult<LineChartDto>> GetLineChartAmounts()
        {
            string StoredProc = @"Exec [dbo].[Sp_LineChart] ";
            var lineChartAmounts = await _unitOfWork.Repository<Sp_LineChart>().StoredProc(StoredProc);
            return _mapper.Map<LineChartDto>(lineChartAmounts.FirstOrDefault());
        }

    }
}
