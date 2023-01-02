using Core.Entities;
using System.Linq;
using static API.Helpers.EnumData;

namespace Core.Specifications
{
    public class ExpenseListSpecification : BaseSpecifcation<Expense>
    {
        public ExpenseListSpecification(ExpenseSpecParams ExpenseParams) : base(x =>
       !x.Deleted &&
          (string.IsNullOrEmpty(ExpenseParams.Search) ||
        x.Description.ToLower().Contains(ExpenseParams.Search) ||
        x.Id.ToString().ToLower().Contains(ExpenseParams.Search) ||
        x.ExpenseType.Name.ToLower().Contains(ExpenseParams.Search) ||
        x.CreateUser.FirstName.ToLower().Contains(ExpenseParams.Search) ||
        x.CreateUser.LastName.ToLower().Contains(ExpenseParams.Search) 
        ) &&
          (!ExpenseParams.TransactionFromDate.HasValue || x.TransactionDate >= ExpenseParams.TransactionFromDate)&&
         (!ExpenseParams.TransactionToDate.HasValue || x.CreateDate <= ExpenseParams.TransactionToDate))
        {
            AddInclude(x => x.CreateUser);
            AddInclude(x => x.ExpenseType);
            AddOrderByDescending(x => x.CreateDate);
            ApplyPaging(ExpenseParams.PageSize * (ExpenseParams.PageIndex - 1), ExpenseParams.PageSize);

            if (!string.IsNullOrEmpty(ExpenseParams.Sort))
            {
                switch (ExpenseParams.Sort)
                {
                    case "idAsc":
                        AddOrderBy(p => p.Id);
                        break;
                    case "idDesc":
                        AddOrderByDescending(p => p.Id);
                        break;
                    default:
                        AddOrderByDescending(n => n.CreateDate);
                        break;
                }
            }

        }

        public ExpenseListSpecification(int ExpenseId) : base(x => x.Id == ExpenseId)
        {
            AddInclude(x => x.CreateUser);
            AddInclude(x => x.ExpenseType);
        }

    }
}
