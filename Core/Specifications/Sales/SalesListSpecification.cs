using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class SalesListSpecification : BaseSpecifcation<SalesInvoice>
    {
        public SalesListSpecification(SalesSpecParams salesParams) : base(x =>
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
            AddInclude(x=>x.Client);
            AddInclude(x => x.SalesInvoiceProducts);
            AddInclude(x => x.SalesInvoicePayments);
            AddOrderByDescending(x => x.CreateDate);
            ApplyPaging(salesParams.PageSize * (salesParams.PageIndex - 1), salesParams.PageSize);

            if (!string.IsNullOrEmpty(salesParams.Sort))
            {
                switch (salesParams.Sort)
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

        public SalesListSpecification(int id) : base(x => x.Id == id)
        {
            AddInclude(x => x.Client);
            AddInclude(x => x.SalesInvoiceProducts);
            AddInclude(x => x.SalesInvoicePayments);

        }
    }
}
