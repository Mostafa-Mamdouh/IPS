using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class PurchaseInvoiceDto
    {
        public int Id { get; set; }

        [Required (ErrorMessage ="Supplier is required")]
        public int SupplierId { get; set; }
        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Invoice Date is required")]
        public DateTime InvoiceDate { get; set; }
        public string PurchaseInvoiceNumber { get; set; }
        public string Note { get; set; }
        [Required(ErrorMessage = "Tax % is required")]
        public decimal Tax { get; set; }
        [Required(ErrorMessage = "Transfer % is required")]
        public decimal Transfer { get; set; }
        public decimal? AdditionalFees { get; set; }

        public int? ArchiveId { get; set; }
        public bool IsTax { get; set; }
        public ICollection<InvoiceProductDto> invoiceProducts { get; set; }

        public ICollection<int> deletedIds { get; set; }


    }
}