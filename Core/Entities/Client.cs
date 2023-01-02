using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Client : BaseEntity
    {
        public Client()
        {
            SalesInvoices = new HashSet<SalesInvoice>();
        }

        public string Name { get; set; }
        public int CityId { get; set; }
        public string Address { get; set; }
        public string TaxReferenceNumber { get; set; }
        public string RepresentativeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }

        public City City { get; set; }
        public AppUser CreateUser { get; set; }
        public AppUser UpdateUser { get; set; }
        public  ICollection<SalesInvoice> SalesInvoices { get; set; }

    }
}
