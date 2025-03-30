


namespace Systemize.Models.ViewModel.Workflow
{
    public class WorkflowEntire
    {


        public Systemize.Models.Workflow Workflow { get; set; }

        public bool isReadonly { get; set; }

        public List<AvailableActions>? Actions { get; set; }

    }
}
