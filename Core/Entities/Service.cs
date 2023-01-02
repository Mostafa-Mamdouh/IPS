using System.Collections.Generic;


namespace Core.Entities
{
    public  class Service : BaseEntity
    {
        public Service()
        {
            PurchaseInvoiceProducts = new HashSet<PurchaseInvoiceProduct>();
            SalesInvoiceProducts = new HashSet<SalesInvoiceProduct>();
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public AppUser CreateUser { get; set; }
        public AppUser UpdateUser { get; set; }
        public  ICollection<PurchaseInvoiceProduct> PurchaseInvoiceProducts { get; set; }
        public  ICollection<SalesInvoiceProduct> SalesInvoiceProducts { get; set; }
    }
}
