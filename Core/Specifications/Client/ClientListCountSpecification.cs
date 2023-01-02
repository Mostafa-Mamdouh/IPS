using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class ClientListCountSpecification : BaseSpecifcation<Client>
    {
        public ClientListCountSpecification(ClientSpecParams clientParams) : base(x =>
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
            

        }

  
    }
}
