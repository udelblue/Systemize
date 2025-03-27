using System.ComponentModel.DataAnnotations;

namespace Systemize.Models
{
    public class WorkflowTag
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
