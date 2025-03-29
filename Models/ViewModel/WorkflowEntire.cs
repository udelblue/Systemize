namespace Systemize.Models.ViewModel
{
    public class WorkflowEntire
    {


        public Workflow Workflow { get; set; }

        public bool isReadonly { get; set; }

        public List<AvailableActions>? Actions { get; set; }

    }
}
