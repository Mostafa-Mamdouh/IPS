using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InvoiceProductDto
    {
        public int Id { get; set; }
        public int? SalesInvoiceId { get; set; }
        public int? PurchaseInvoiceId { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceId { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Price is required")]
        public decimal Price { get; set; }


    }
}
