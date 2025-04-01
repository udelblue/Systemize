using System.ComponentModel.DataAnnotations;

namespace Systemize.Models
{
    public class DocumentTag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
