using Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Sp_PurchaseInvoiceReport :BaseEntity
    {
        public new int Id { get; set; }
        public string SupplierName { get; set; }
        public string TaxReferenceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        public string Products { get; set; }
        public string Services { get; set; }
        public string Note { get; set; }
        public decimal TotalProductCost { get; set; }
        public decimal TotalServiceCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal TransferPercentage { get; set; }
        public decimal TransferAmount { get; set; }
        public decimal AdditionalFees { get; set; }
        public decimal TotalInvoice { get; set; }
        public new  DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public string PaymentStatus { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal Remaining { get; set; }
        public string InvoiceValidaty { get; set; }
        public string Type { get; set; }

        [NotMapped]
        public new int CreateUserId { get; set; }
        [NotMapped]
        public new int UpdateUserId { get; set; }
        [NotMapped]
        public new DateTime UpdateDate { get; set; }
        [NotMapped]
        public new bool Deleted { get; set; } 

    }
}
