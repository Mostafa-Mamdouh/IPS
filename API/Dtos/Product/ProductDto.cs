using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ProductDto
    {
        public int Id { get; set; }
        [Required (ErrorMessage ="Product name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }
    }
}