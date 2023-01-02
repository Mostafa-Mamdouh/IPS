using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Sp_ProductSearch : BaseEntity
    {
        public string ProductName { get; set; }
        public string Code { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int Inbound { get; set; }
        public int Outbound { get; set; }
        public int Stock { get; set; }
        public string CreatedBy { get; set; }
        public int TotalCount { get; set; }

        public string DisplayValue { get; set; }


    }
}
