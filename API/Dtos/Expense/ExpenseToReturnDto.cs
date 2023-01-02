using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ExpenseToReturnDto
    {
        public int Id { get; set; }
        public int ExpenseTypeId { get; set; }
        public string ExpenseType { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int PaymentMethod { get; set; }
        public string TransferNumber { get; set; }
        public string ChequeNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }

    }
}
