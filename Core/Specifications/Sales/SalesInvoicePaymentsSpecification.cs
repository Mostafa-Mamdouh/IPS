using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class SalesInvoicePaymentsSpecification : BaseSpecifcation<SalesInvoicePayment>
    {
        public SalesInvoicePaymentsSpecification(int invoiceId) : base(x => x.SalesInvoiceId == invoiceId)
        {
            AddInclude(x => x.Archive);
        }
    }
}
