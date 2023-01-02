using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class SalesPaymentSpecification : BaseSpecifcation<SalesInvoicePayment>
    {
        public SalesPaymentSpecification(int invoiceId,bool byInvoice) : base(x =>x.SalesInvoiceId==invoiceId)
        {
        }

        public SalesPaymentSpecification(int paymentId) : base(x => x.Id == paymentId )
        {
            AddInclude(x => x.Archive);
        }
    }
}
