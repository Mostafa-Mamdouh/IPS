
namespace Core.Entities
{
    public  class SalesInvoiceProduct : BaseEntity
    {
        public int SalesInvoiceId { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public virtual Product Product { get; set; }
        public virtual SalesInvoice SalesInvoice { get; set; }
        public virtual Service Service { get; set; }
    }
}
