using System;
using System.Collections.Generic;
using static API.Helpers.EnumData;

namespace Core.Entities
{
    public class PurchaseInvoicePayment : BaseEntity
    {
        public int PurchaseInvoiceId { get; set; }
        public PaymentType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ChequeNumber { get; set; }
        public string TransferNumber { get; set; }
        public int? ArchiveId { get; set; }

        public SystemArchive Archive { get; set; }
        public  AppUser CreateUser { get; set; }
        public  PurchaseInvoice PurchaseInvoice { get; set; }
        public AppUser UpdateUser { get; set; }
    }
}
