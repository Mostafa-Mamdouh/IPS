using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class PurchaseInvoiceProductsSpecification : BaseSpecifcation<PurchaseInvoiceProduct>
    {
        public PurchaseInvoiceProductsSpecification(int invoiceId) : base(x => x.PurchaseInvoiceId == invoiceId)
        {
            AddInclude(x => x.Product);
            AddInclude(x => x.Service);
        }
    }
}
