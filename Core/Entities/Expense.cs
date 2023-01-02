using System;
using System.Collections.Generic;
using static API.Helpers.EnumData;

namespace Core.Entities
{
    public class Expense : BaseEntity
    {
        public int ExpenseTypeId  { get; set; }
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string TransferNumber { get; set; }
        public string ChequeNumber { get; set; }
        public ExpenseType ExpenseType { get; set; }
        public AppUser CreateUser { get; set; }
        public AppUser UpdateUser { get; set; }

    }
}
