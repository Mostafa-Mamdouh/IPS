using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class InvoiceToReturnDto
    {
        public int Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string TaxReferenceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string PurchaseInvoiceNumber { get; set; }
        public int? SalesInvoiceNumber { get; set; }
        public string BrokerName { get; set; }
        public string Note { get; set; }
        public decimal Tax { get; set; }
        public decimal Transfer { get; set; }
        public decimal? AdditionalFees { get; set; }
        public decimal? Transportaion { get; set; }
        public decimal TotalInvoice { get; set; }
        public decimal TotalPaid { get; set; }
        public int? ArchiveId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public int CreateUserId{ get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }
        public bool Deleted { get; set; }
        public bool IsTax { get; set; }

        public ICollection<InvoicePaymentToReturnDto> InvoicePayments { get; set; }
        public  ICollection<InvoiceProductToReturnDto> InvoiceProducts { get; set; }


    }
}
