using System.ComponentModel.DataAnnotations;

namespace Systemize.Models
{
    public class WorkflowNote
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [Required]
        public string Author { get; set; }
    }
}
