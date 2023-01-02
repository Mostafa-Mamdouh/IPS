using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class SalesInvoiceProductsSpecification : BaseSpecifcation<SalesInvoiceProduct>
    {
        public SalesInvoiceProductsSpecification(int invoiceId) : base(x => x.SalesInvoiceId == invoiceId)
        {
            AddInclude(x => x.Product);
            AddInclude(x => x.Service);
        }
    }
}
