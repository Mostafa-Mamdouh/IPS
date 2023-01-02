using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class SP_BankLatestTransactions : BaseEntity
    {
        public int PaymentId { get; set; }
        public string Type { get; set; }
        public string PurchaseInvoiceNumber { get; set; }
        public int? SalesInvoiceNumber { get; set; }
        public int InvoiceId { get; set; }
        public string BrokerName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Credit { get; set; }
        public decimal Debit { get; set; }
        public decimal Current { get; set; }
        public int Total_Count { get; set; }

       

    }
}
