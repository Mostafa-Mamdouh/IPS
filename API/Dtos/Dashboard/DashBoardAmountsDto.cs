using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class DashBoardAmountsDto
    {
        public decimal UnpaidSalesAmount { get; set; }
        public decimal UnpaidPurchaseAmount { get; set; }
        public decimal TotalSalesAmount { get; set; }

    }
}
