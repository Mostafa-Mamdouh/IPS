using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class ExpenseType : BaseEntity
    {
        public ExpenseType()
        {
            Expenses = new HashSet<Expense>();
        }
        public string Name { get; set; }
        public  ICollection<Expense> Expenses { get; set; }
    }
}
