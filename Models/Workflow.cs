using System.ComponentModel.DataAnnotations;


namespace Systemize.Models
{
    public class Workflow
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }


        public DateTime? CreatedOn { get; set; }




        public List<string>? CreatedBy { get; set; }

        public List<string>? AssignedTo { get; set; }

        [ConcurrencyCheck]
        public int? CurrentStageId { get; set; }
        [ConcurrencyCheck]
        public string? CurrentStageName { get; set; }

        public int? PercentageComplete { get; set; }

        public string? Status { get; set; }

        public string? FormData { get; set; } = "[]";


        public WorkflowForm? WorkflowForm { get; set; }


        public List<Stage> Stages { get; set; }

        public List<Document> Documents { get; set; }

        public List<Link> Links { get; set; }

        public List<WorkflowTag> Tags { get; set; }
        public virtual List<History> History { get; set; }

        public virtual List<WorkflowNote> Notes { get; set; }


    }
}
