using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class PurchaseInvoiceProductsByProductIdSpecification : BaseSpecifcation<PurchaseInvoiceProduct>
    {
        public PurchaseInvoiceProductsByProductIdSpecification(int productId) : base(x => x.ProductId == productId)
        {
        }
    }
}
