using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class SalesInvoiceProductsByServiceIdSpecification : BaseSpecifcation<SalesInvoiceProduct>
    {
        public SalesInvoiceProductsByServiceIdSpecification(int serviceId) : base(x => x.ServiceId == serviceId)
        {
        }
    }
}
