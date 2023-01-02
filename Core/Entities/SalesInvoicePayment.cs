using System;
using static API.Helpers.EnumData;

namespace Core.Entities
{
    public class SalesInvoicePayment : BaseEntity
    {
        public int SalesInvoiceId { get; set; }
        public PaymentType Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string ChequeNumber { get; set; }
        public string TransferNumber { get; set; }
        public int? ArchiveId { get; set; }

        public virtual SystemArchive Archive { get; set; }
        public virtual AppUser CreateUser { get; set; }
        public virtual SalesInvoice SalesInvoice { get; set; }
        public virtual AppUser UpdateUser { get; set; }
    }
}
