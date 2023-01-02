using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class SupplierListSpecification : BaseSpecifcation<Supplier>
    {
        public SupplierListSpecification(SupplierSpecParams supplierParams) : base(x =>
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
            AddInclude(x => x.City);
            AddInclude(x => x.CreateUser);
            AddOrderByDescending(x => x.CreateDate);
            ApplyPaging(supplierParams.PageSize * (supplierParams.PageIndex - 1), supplierParams.PageSize);

            if (!string.IsNullOrEmpty(supplierParams.Sort))
            {
                switch (supplierParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(p => p.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    case "idAsc":
                        AddOrderBy(p => p.Id);
                        break;
                    case "idDesc":
                        AddOrderByDescending(p => p.Id);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }

        }

        public SupplierListSpecification(int supplierId) : base(x => x.Id == supplierId)
        {
            AddInclude(x => x.City);
            AddInclude(x => x.CreateUser);
        }
        public SupplierListSpecification(string name,int id,string taxRefNo) : base(x => x.TaxReferenceNumber == taxRefNo && x.Id!=id)
        {
        }
    }
}
