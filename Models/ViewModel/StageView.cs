namespace Systemize.Models.ViewModel
{
    public class StageView
    {

        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string StageType { get; set; }

        // completed, skipped, current, 
        public string Status { get; set; }
    }
}
