using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class ClientListSpecification : BaseSpecifcation<Client>
    {
        public ClientListSpecification(ClientSpecParams clientParams) : base(x =>
          !x.Deleted &&
          (string.IsNullOrEmpty(clientParams.Search) ||
        x.Name.ToLower().Contains(clientParams.Search) ||
        x.Id.ToString().ToLower().Contains(clientParams.Search) ||
        x.City.Name.ToLower().Contains(clientParams.Search) ||
        x.Address.ToLower().Contains(clientParams.Search) ||
        x.TaxReferenceNumber.ToLower().Contains(clientParams.Search) ||
        x.Email.ToLower().Contains(clientParams.Search) ||
        x.RepresentativeName.ToLower().Contains(clientParams.Search) ||
        x.CreateUser.FirstName.ToLower().Contains(clientParams.Search) ||
        x.CreateUser.LastName.ToLower().Contains(clientParams.Search) ||
        x.MobileNumber.ToLower().Contains(clientParams.Search) 


        ) &&
          (!clientParams.CreateFromDate.HasValue || x.CreateDate >= clientParams.CreateFromDate) &&
          (!clientParams.CreateToDate.HasValue || x.CreateDate <= clientParams.CreateToDate) &&
          (!clientParams.InvoiceFromDate.HasValue || x.SalesInvoices.Any(s => s.InvoiceDate >= clientParams.InvoiceFromDate)) &&
          (!clientParams.InvoiceToDate.HasValue || x.SalesInvoices.Any(s => s.InvoiceDate <= clientParams.InvoiceToDate))

        )
        {
            AddInclude(x => x.City);
            AddInclude(x => x.CreateUser);
            AddOrderByDescending(x => x.CreateDate);
            ApplyPaging(clientParams.PageSize * (clientParams.PageIndex - 1), clientParams.PageSize);

            if (!string.IsNullOrEmpty(clientParams.Sort))
            {
                switch (clientParams.Sort)
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
                        AddOrderByDescending(n => n.CreateDate);
                        break;
                }
            }

        }

        public ClientListSpecification(int clientId) : base(x => x.Id == clientId)
        {
            AddInclude(x => x.City);
            AddInclude(x => x.CreateUser);
        }

        public ClientListSpecification(string name, int id,string taxRefNo) : base(x => (x.TaxReferenceNumber == taxRefNo) && x.Id!=id)
        {
        }
    }
}
