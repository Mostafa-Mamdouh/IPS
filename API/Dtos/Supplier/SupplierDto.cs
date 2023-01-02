using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class SupplierDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "City Is Required")]
        public int CityId { get; set; }
        public string Address { get; set; }
        public string TaxReferenceNumber { get; set; }
        [Required(ErrorMessage = "Representative Name Is Required")]
        public string RepresentativeName { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Mobile Number Is Required")]
        [DataType(DataType.PhoneNumber)]
        public string MobileNumber { get; set; }
    }
}