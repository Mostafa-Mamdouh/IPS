using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class SalesInvoiceProductsByProductIdSpecification : BaseSpecifcation<SalesInvoiceProduct>
    {
        public SalesInvoiceProductsByProductIdSpecification(int productId) : base(x => x.ProductId == productId)
        {
        }
    }
}
