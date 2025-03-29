using System.ComponentModel.DataAnnotations;

namespace Systemize.Models
{
    public class WorkflowTemplate
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        public List<Stage>? Stages { get; set; }

    }
}
