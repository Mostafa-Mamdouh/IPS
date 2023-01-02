using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            Products = new HashSet<Product>();
            Services = new HashSet<Service>();
        }
        public string Name { get; set; }
        public  ICollection<Product> Products { get; set; }
        public  ICollection<Service> Services { get; set; }
    }
}
