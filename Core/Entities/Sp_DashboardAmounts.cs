using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Sp_DashboardAmounts : BaseEntity
    {
        public decimal UnpaidSalesAmount { get; set; }
        public decimal UnpaidPurchaseAmount { get; set; }
        public decimal TotalSalesAmount { get; set; }

    }
}
