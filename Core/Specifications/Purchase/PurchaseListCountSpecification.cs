using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class PurchaseListCountSpecification : BaseSpecifcation<PurchaseInvoice>
    {
        public PurchaseListCountSpecification(PurchaseSpecParams purchaseParams) : base(x =>
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
            

        }

  
    }
}
