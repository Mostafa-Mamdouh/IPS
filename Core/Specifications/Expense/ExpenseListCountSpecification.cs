using Core.Entities;
using System.Linq;
using static API.Helpers.EnumData;

namespace Core.Specifications
{
    public class ExpenseListCountSpecification : BaseSpecifcation<Expense>
    {
        public ExpenseListCountSpecification(ExpenseSpecParams ExpenseParams) : base(x =>
        !x.Deleted &&
         (string.IsNullOrEmpty(ExpenseParams.Search) ||
        x.Description.ToLower().Contains(ExpenseParams.Search) ||
        x.Id.ToString().ToLower().Contains(ExpenseParams.Search) ||
        x.ExpenseType.Name.ToLower().Contains(ExpenseParams.Search) ||
        x.CreateUser.FirstName.ToLower().Contains(ExpenseParams.Search) ||
        x.CreateUser.LastName.ToLower().Contains(ExpenseParams.Search) 
        ) &&
          (!ExpenseParams.TransactionFromDate.HasValue || x.TransactionDate >= ExpenseParams.TransactionFromDate) &&
          (!ExpenseParams.TransactionToDate.HasValue || x.CreateDate <= ExpenseParams.TransactionToDate) )
        {
            

        }

  
    }
}
