using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class PurchasePaymentSpecification : BaseSpecifcation<PurchaseInvoicePayment>
    {
        public PurchasePaymentSpecification(int invoiceId,bool byInvoice) : base(x =>x.PurchaseInvoiceId==invoiceId)
        {
        }

        public PurchasePaymentSpecification(int paymentId) : base(x => x.Id == paymentId )
        {
            AddInclude(x => x.Archive);
        }
    }
}
