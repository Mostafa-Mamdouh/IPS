using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class SalesInvoiceByClientIdSpecification : BaseSpecifcation<SalesInvoice>
    {
        public SalesInvoiceByClientIdSpecification(int clientId) : base(x => x.ClientId == clientId)
        {
        }
    }
}
