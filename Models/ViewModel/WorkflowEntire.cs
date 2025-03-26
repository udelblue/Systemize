namespace Systemize.Models.ViewModel
{
    public class WorkflowEntire
    {

        public string? UserName { get; set; }

        public Workflow Workflow { get; set; }

        public List<AvailableActions>? Actions { get; set; }

    }
}
