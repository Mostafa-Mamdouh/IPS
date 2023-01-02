using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InvoicePaymentToReturnDto
    {
        public int Id { get; set; }
        public int? PurchaseInvoiceId { get; set; }
        public int? SalesInvoiceId { get; set; }

        public int Type { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int PaymentMethod { get; set; }
        public string ChequeNumber { get; set; }
        public string TransferNumber { get; set; }
        public int? ArchiveId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int CreateUserId { get; set; }
        public string CreatedBy { get; set; }
        public bool Deleted { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
