using API.ModelValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InvoicePaymentDto
    {
        public int Id { get; set; }
        public int? SalesInvoiceId { get; set; }
        public int? PurchaseInvoiceId { get; set; }
        [HasValueValidation("PurchaseInvoiceId", ErrorMessage ="Payment type is required")]
        public int? Type { get; set; }
        [Required(ErrorMessage = "Payment amount is required")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Payment date is required")]
        [DataType(DataType.DateTime)]
        public DateTime PaymentDate { get; set; }
        [Required(ErrorMessage = "Payment Method is required")]
        public int PaymentMethod { get; set; }

        [RequiredIfValidation("PaymentMethod", 2, ErrorMessage = "Cheque Number is required")]
        public string ChequeNumber { get; set; }
        [RequiredIfValidation("PaymentMethod", 3, ErrorMessage = "Transfer Number is required")]
        public string TransferNumber { get; set; }
        public int? ArchiveId { get; set; }
    }
}
