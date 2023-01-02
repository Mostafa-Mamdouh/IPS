using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InvoiceProductToReturnDto
    {
        public int Id { get; set; }
        public int? PurchaseInvoiceId { get; set; }
        public int? SalesInvoiceId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? ServiceId { get; set; }
        public string ServiceName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool Deleted { get; set; }

    }
}
