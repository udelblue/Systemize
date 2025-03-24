using System.ComponentModel.DataAnnotations;

namespace Systemize.Models
{

    public class Document
    {
        [Key]
        public int DocumentID { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        public byte[] Content { get; set; }
        public string ContentType { get; set; }


    }

}
