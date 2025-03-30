using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Systemize.Models
{




    public class History
    {
        public History()
        {

        }

        public History(String executedBy, String action, string description)
        {
            ExecutedBy = executedBy;
            ExecutedAt = DateTime.Now;
            Action = action;
            StageId = null;
            Description = description;

        }

        public History(String executedBy, String action, int? stageid, string description)
        {
            ExecutedBy = executedBy;
            ExecutedAt = DateTime.Now;
            Action = action;
            StageId = stageid;
            Description = description;

        }

        public History(String executedBy, String action, int? stageid, string eventlevel, string eventname, string description)
        {
            ExecutedBy = executedBy;
            ExecutedAt = DateTime.Now;
            Action = action;
            StageId = stageid;
            Description = description;
            EventLevel = eventlevel;
            EventName = eventname;


        }


        [Key]
        public int Id { get; set; }

        public string? ExecutedBy { get; set; }
        public DateTime ExecutedAt { get; set; }

        public int? StageId { get; set; }

        public string? EventLevel { get; set; }

        public string? EventName { get; set; }

        public string Action { get; set; }

        public string Description { get; set; }

        [ForeignKey("WorkflowId")]
        public int WorkflowId { get; set; }



    }
}
