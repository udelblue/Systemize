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
            StageName = null;
            Description = description;

        }


        public History(String executedBy, String action, int? stageid, string? stageName, string eventlevel, string eventname, string description)
        {
            ExecutedBy = executedBy;
            ExecutedAt = DateTime.Now;
            Action = action;
            StageId = stageid;
            StageName = stageName;
            Description = description;
            EventLevel = eventlevel;
            EventName = eventname;


        }


        [Key]
        public int Id { get; set; }


        //executed by
        public string? ExecutedBy { get; set; }

        // time executed
        public DateTime ExecutedAt { get; set; }


        // stage id
        public int? StageId { get; set; }


        //stage name
        public string? StageName { get; set; }


        // 
        public string? EventLevel { get; set; }

        // start workflow, complete workflow, start stage, complete stage
        public string? EventName { get; set; }

        // action stratagy used
        public string? Action { get; set; }


        // general discription
        public string Description { get; set; }

        [ForeignKey("WorkflowId")]
        public int WorkflowId { get; set; }



    }
}
