using System;
using System.Collections.Generic;

#nullable disable

namespace Core.Entities
{
    public class Country:BaseEntity
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        public string Name { get; set; }

        public ICollection<City> Cities { get; set; }
    }
}
