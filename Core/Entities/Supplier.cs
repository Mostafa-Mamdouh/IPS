
using System.Collections.Generic;

namespace Core.Entities
{
    public class Supplier : BaseEntity
    {
        public Supplier()
        {
            PurchaseInvoices = new HashSet<PurchaseInvoice>();
        }

        public string Name { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string TaxReferenceNumber { get; set; }
        public string RepresentativeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
 
        public  City City { get; set; }
        public  AppUser CreateUser { get; set; }
        public AppUser UpdateUser { get; set; }

        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; }
    }
}
