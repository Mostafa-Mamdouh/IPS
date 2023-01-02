using System;
using System.Collections.Generic;


namespace Core.Entities
{
    public  class City : BaseEntity
    {
        public City()
        {
            Clients = new HashSet<Client>();
            Suppliers = new HashSet<Supplier>();
        }

        public string Name { get; set; }
        public int CountryId { get; set; }
        public  Country Country { get; set; }
        public  ICollection<Client> Clients { get; set; }
        public ICollection<Supplier> Suppliers { get; set; }

    }
}
