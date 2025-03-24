using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Systemize.Models
{
    public class History
    {
        [Key]
        public int Id { get; set; }

        public string ExecutedBy { get; set; }
        public DateTime ExecutedAt { get; set; }

        public int? StageId { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        [ForeignKey("WorkflowId")]
        public int WorkflowId { get; set; }



    }
}
