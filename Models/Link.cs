using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Systemize.Models
{
    public class Link
    {

        [Key]
        public int LinkID { get; set; }

        public string Title { get; set; }

        public string? Description { get; set; }


        public string? URL { get; set; }

        [ForeignKey("WorkflowId")]
        public int WorkflowId { get; set; }

    }
}
