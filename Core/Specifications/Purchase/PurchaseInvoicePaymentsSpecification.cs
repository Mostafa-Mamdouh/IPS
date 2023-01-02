using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class PurchaseInvoicePaymentsSpecification : BaseSpecifcation<PurchaseInvoicePayment>
    {
        public PurchaseInvoicePaymentsSpecification(int invoiceId) : base(x => x.PurchaseInvoiceId == invoiceId)
        {
            AddInclude(x => x.Archive);
        }
    }
}
