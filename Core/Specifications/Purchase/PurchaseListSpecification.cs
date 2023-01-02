using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class PurchaseListSpecification : BaseSpecifcation<PurchaseInvoice>
    {
        public PurchaseListSpecification(PurchaseSpecParams purchaseParams) : base(x =>
           (string.IsNullOrEmpty(purchaseParams.Search) ||
        x.InvoiceNumber.ToLower().Contains(purchaseParams.Search) ||
        x.Id.ToString().ToLower().Contains(purchaseParams.Search) ||
        x.Supplier.Name.ToLower().Contains(purchaseParams.Search) ||
        x.CreateUser.FirstName.ToLower().Contains(purchaseParams.Search) ||
        x.CreateUser.LastName.ToLower().Contains(purchaseParams.Search)


        ) &&
          (!purchaseParams.InvoiceFromDate.HasValue || x.InvoiceDate >= purchaseParams.InvoiceFromDate) &&
          (!purchaseParams.InvoiceToDate.HasValue || x.InvoiceDate <= purchaseParams.InvoiceToDate)

        )
        {
            AddInclude(x => x.Supplier);
            AddInclude(x => x.PurchaseInvoicePayments);
            AddInclude(x => x.PurchaseInvoiceProducts);
            AddOrderByDescending(x => x.CreateDate);
            ApplyPaging(purchaseParams.PageSize * (purchaseParams.PageIndex - 1), purchaseParams.PageSize);

            if (!string.IsNullOrEmpty(purchaseParams.Sort))
            {
                switch (purchaseParams.Sort)
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

        public PurchaseListSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Archive);
            AddInclude(x => x.Supplier);
            AddInclude(x => x.PurchaseInvoiceProducts);
            AddInclude(x => x.PurchaseInvoicePayments);
        }
    }
}
