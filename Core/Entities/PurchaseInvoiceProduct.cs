
namespace Core.Entities
{
    public  class PurchaseInvoiceProduct : BaseEntity
    {
        public int PurchaseInvoiceId { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public  Product Product { get; set; }
        public  PurchaseInvoice PurchaseInvoice { get; set; }
        public  Service Service { get; set; }
    }
}
