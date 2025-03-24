using System.ComponentModel.DataAnnotations;

namespace Systemize.Models
{
    public class Stage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }
        [Required]
        public string StageType { get; set; }

        public string Properties { get; set; }
    }
}
