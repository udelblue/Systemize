namespace Systemize.Models.ViewModel.Workflow
{
    public class DocumentEdit
    {


        public int DocumentID { get; set; }

        public string Title { get; set; }
        public string? Description { get; set; }

        public string? DocumentType { get; set; }

        public string ContentType { get; set; }
    }
}
