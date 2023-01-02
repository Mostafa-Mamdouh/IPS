using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class SupplierListCountSpecification : BaseSpecifcation<Supplier>
    {
        public SupplierListCountSpecification(SupplierSpecParams supplierParams) : base(x =>
     !x.Deleted &&
          (string.IsNullOrEmpty(supplierParams.Search) ||
        x.Name.ToLower().Contains(supplierParams.Search) ||
        x.Id.ToString().ToLower().Contains(supplierParams.Search) ||
        x.City.Name.ToLower().Contains(supplierParams.Search) ||
        x.Address.ToLower().Contains(supplierParams.Search) ||
        x.TaxReferenceNumber.ToLower().Contains(supplierParams.Search) ||
        x.Email.ToLower().Contains(supplierParams.Search) ||
        x.RepresentativeName.ToLower().Contains(supplierParams.Search) ||
        x.CreateUser.FirstName.ToLower().Contains(supplierParams.Search) ||
        x.CreateUser.LastName.ToLower().Contains(supplierParams.Search) ||
        x.MobileNumber.ToLower().Contains(supplierParams.Search)


        ) &&
          (!supplierParams.CreateFromDate.HasValue || x.CreateDate >= supplierParams.CreateFromDate) &&
          (!supplierParams.CreateToDate.HasValue || x.CreateDate <= supplierParams.CreateToDate) &&
          (!supplierParams.InvoiceFromDate.HasValue || x.PurchaseInvoices.Any(s => s.InvoiceDate >= supplierParams.InvoiceFromDate)) &&
          (!supplierParams.InvoiceToDate.HasValue || x.PurchaseInvoices.Any(s => s.InvoiceDate <= supplierParams.InvoiceToDate))

        )
        {
            

        }

  
    }
}
