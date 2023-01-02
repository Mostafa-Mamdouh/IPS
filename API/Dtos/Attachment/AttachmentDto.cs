using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class AttachmentDto
    {
        public int Id { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }
}