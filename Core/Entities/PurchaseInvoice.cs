using System;
using System.Collections.Generic;


namespace Core.Entities
{
    public class PurchaseInvoice : BaseEntity
    {
        public PurchaseInvoice()
        {
            PurchaseInvoicePayments = new HashSet<PurchaseInvoicePayment>();
            PurchaseInvoiceProducts = new HashSet<PurchaseInvoiceProduct>();
        }

        public int SupplierId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string Note { get; set; }
        public decimal Tax { get; set; }
        public decimal Transfer { get; set; }
        public decimal? AdditionalFees { get; set; }
        public int? ArchiveId { get; set; }
        public bool IsTax { get; set; }

        public Supplier Supplier { get; set; }
        public SystemArchive Archive { get; set; }
        public AppUser CreateUser { get; set; }
        public AppUser UpdateUser { get; set; }
        public ICollection<PurchaseInvoicePayment> PurchaseInvoicePayments { get; set; }
        public ICollection<PurchaseInvoiceProduct> PurchaseInvoiceProducts { get; set; }
    }
}
