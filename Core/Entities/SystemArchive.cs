using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class SystemArchive : BaseEntity
    {
        public SystemArchive()
        {
            PurchaseInvoicePayments = new HashSet<PurchaseInvoicePayment>();
            PurchaseInvoices = new HashSet<PurchaseInvoice>();
            SalesInvoicePayments = new HashSet<SalesInvoicePayment>();
        }

        public string FilePath { get; set; }
        public string FileName { get; set; }

        public  ICollection<PurchaseInvoicePayment> PurchaseInvoicePayments { get; set; }
        public  ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
        public  ICollection<SalesInvoicePayment> SalesInvoicePayments { get; set; }
    }
}
