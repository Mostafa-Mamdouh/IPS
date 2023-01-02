using API.ModelValidations;
using System;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class CreditDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bank Credit is required")]
        public decimal BankCredit { get; set; }
        [Required(ErrorMessage = "Cash Credit is required")]
        public decimal CashCredit { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreateDate { get; set; }

    }
}