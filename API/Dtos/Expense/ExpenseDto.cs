using API.ModelValidations;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ExpenseDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Expenses Type is required")]
        public int ExpenseTypeId { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "TransactionDate date is required")]
        [DataType(DataType.DateTime)]
        public DateTime TransactionDate { get; set; }
        [Required(ErrorMessage = "Amount is required")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "Payment Method is required")]
        public int PaymentMethod { get; set; }

        [RequiredIfValidation("PaymentMethod", 2, ErrorMessage = "Cheque Number is required")]
        public string ChequeNumber { get; set; }
        [RequiredIfValidation("PaymentMethod", 3, ErrorMessage = "Transfer Number is required")]
        public string TransferNumber { get; set; }
    }
}