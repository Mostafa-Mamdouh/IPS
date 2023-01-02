using System;
using System.Collections.Generic;


namespace Core.Entities
{
    public class SalesInvoice : BaseEntity
    {
        public SalesInvoice()
        {
            SalesInvoicePayments = new HashSet<SalesInvoicePayment>();
            SalesInvoiceProducts = new HashSet<SalesInvoiceProduct>();
        }

        public int ClientId { get; set; }
        public int? InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string BrokerName { get; set; }
        public string Note { get; set; }
        public decimal Tax { get; set; }
        public decimal Transfer { get; set; }
        public decimal? Transportation { get; set; }
        public bool IsTax { get; set; }
        public Client Client { get; set; }
        public  AppUser CreateUser { get; set; }
        public  AppUser UpdateUser { get; set; }
        public  ICollection<SalesInvoicePayment> SalesInvoicePayments { get; set; }
        public  ICollection<SalesInvoiceProduct> SalesInvoiceProducts { get; set; }
    }
}
