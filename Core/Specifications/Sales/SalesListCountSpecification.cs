using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class SalesListCountSpecification : BaseSpecifcation<SalesInvoice>
    {
        public SalesListCountSpecification(SalesSpecParams salesParams) : base(x =>
             (string.IsNullOrEmpty(salesParams.Search) ||
        x.BrokerName.ToLower().Contains(salesParams.Search) ||
        x.InvoiceNumber.ToString().ToLower().Contains(salesParams.Search) ||
        x.Client.Name.ToLower().Contains(salesParams.Search) ||
        x.CreateUser.FirstName.ToLower().Contains(salesParams.Search) ||
        x.CreateUser.LastName.ToLower().Contains(salesParams.Search)


        ) &&
          (!salesParams.InvoiceFromDate.HasValue || x.InvoiceDate >= salesParams.InvoiceFromDate) &&
          (!salesParams.InvoiceToDate.HasValue || x.InvoiceDate <= salesParams.InvoiceToDate)
        )
        {
            

        }

  
    }
}
