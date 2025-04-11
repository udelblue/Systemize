using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Systemize.Models
{
    public class Stage
    {



        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }


        public string? Description { get; set; }

        public string? StageType { get; set; }

        public List<string>? AssignedTo { get; set; }


        // completed, skipped, current, 
        public string? StageStatus { get; set; }

        public string? Properties { get; set; }







        [ForeignKey("WorkflowId")]
        public int? WorkflowId { get; set; }


        [ForeignKey("WorkflowTemplateId")]
        public int? WorkflowTemplateId { get; set; }

    }
}
