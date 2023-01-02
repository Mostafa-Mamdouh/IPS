using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class PurchaseInvoiceProductsByServiceIdSpecification : BaseSpecifcation<PurchaseInvoiceProduct>
    {
        public PurchaseInvoiceProductsByServiceIdSpecification(int serviceId) : base(x => x.ServiceId == serviceId)
        {
        }
    }
}
