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

        public string? CurrentlyAssigned { get; set; }
        public int? CurrentStageId { get; set; }

        public string? CurrentStageName { get; set; }

        public int? PercentageComplete { get; set; }

        public string? Status { get; set; }



        public WorkflowForm? WorkflowForm { get; set; }


        public List<Stage> Stages { get; set; }

        public List<Document> Documents { get; set; }

        public List<Link> Links { get; set; }

        public List<WorkflowTag> Tags { get; set; }
        public virtual List<History> History { get; set; }




    }
}
