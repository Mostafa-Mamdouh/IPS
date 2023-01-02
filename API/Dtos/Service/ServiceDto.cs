using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ServiceDto
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="Service name is required")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
    }
}