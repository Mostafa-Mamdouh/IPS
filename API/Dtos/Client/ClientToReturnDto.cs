using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ClientToReturnDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }
        public string Address { get; set; }
        public string TaxReferenceNumber { get; set; }
        public string RepresentativeName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreateDate { get; set; }




    }
}
