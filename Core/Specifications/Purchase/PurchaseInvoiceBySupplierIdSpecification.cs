using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Specifications
{
   public class PurchaseInvoiceBySupplierIdSpecification : BaseSpecifcation<PurchaseInvoice>
    {
        public PurchaseInvoiceBySupplierIdSpecification(int supplierId) : base(x => x.SupplierId == supplierId)
        {
        }
    }
}
