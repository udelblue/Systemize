using System.ComponentModel.DataAnnotations;

namespace Systemize.Models
{
    public class WorkflowTag
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
