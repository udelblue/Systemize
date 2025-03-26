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

        public string? Status { get; set; }

        public List<Stage> Stages { get; set; }

        public List<Document> Documents { get; set; }

        public virtual List<History> History { get; set; }


    }
}
