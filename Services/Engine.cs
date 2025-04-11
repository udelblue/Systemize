using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;
using Systemize.Services.ActionStratagies;

namespace Systemize.Services
{
    public class Engine
    {
        private Workflow _workflow;
        private readonly SystemizeContext _context;


        public Engine(SystemizeContext context, Workflow workflow)
        {

            _context = context;
            _workflow = workflow;
        }




        public Workflow Process(ActionResponse actionRespone)
        {
            IActionProcess process = null;





            switch (actionRespone.ActionType.ToLower())
            {
                case "start":
                    Console.WriteLine("start");
                    process = new StartStratagy(_context);
                    _workflow = process.Execute(_workflow, actionRespone);
                    break;
                case "approval":
                    Console.WriteLine("approval");
                    process = new ApprovalStratagy(_context);
                    _workflow = process.Execute(_workflow, actionRespone);
                    break;
                case "deny":
                    Console.WriteLine("deny");
                    process = new DenyStratagy(_context);
                    _workflow = process.Execute(_workflow, actionRespone);
                    break;

                case "cancel":
                    Console.WriteLine("cancel");
                    process = new CancelStratagy(_context);
                    _workflow = process.Execute(_workflow, actionRespone);
                    break;
                case "previous":
                    Console.WriteLine("previous");
                    process = new PreviousStratagy(_context);
                    _workflow = process.Execute(_workflow, actionRespone);
                    break;
                default:
                    Console.WriteLine("Invalid Action");
                    throw new Exception("Invalid action requested from Engine. " + "Action requested:" + actionRespone.ActionType);
                    break;
            }

            return _workflow;





        }

        public WorkflowEntire AvailableActions(Workflow workflow, String currentUser)
        {
            WorkflowEntire workflowEntire = new WorkflowEntire();
            workflowEntire.isReadonly = workflow.Status != null && workflow.Status.ToLower() == "completed" | workflow.Status.ToLower() == "denied" | workflow.Status.ToLower() == "cancelled" ? true : false;

            //workflow not started
            if (workflow.Status != null && workflow.Status.ToLower() != "completed" && workflow.CurrentStageId == null && workflow.Stages != null && workflow.Stages.Count > 0)
            {
                AvailableActions cancel = new AvailableActions("Cancel", "Cancel workflow.", "Cancel", "btn-warning");
                AvailableActions start = new AvailableActions("Start", "Start the workflow to begin at first stage.", "Start", "btn-success");
                List<AvailableActions> actions = new List<AvailableActions>();

                actions.Add(cancel);
                actions.Add(start);

                workflowEntire.Actions = actions;
            }
            //workflow already started
            else if (workflow.Status != null && workflow.Status.ToLower() != "completed" && workflow.Status.ToLower() != "denied" && workflow.Status.ToLower() != "draft" && workflow.Status.ToLower() != "cancelled")
            {

                int current_index = workflow.Stages.FindIndex(s => s.Id == workflow.CurrentStageId);

                //check if is not first stage
                if (current_index != 0)
                {

                    AvailableActions approval = new AvailableActions("Approval", "Approval of stage and moves to next stage.", "Approval", "btn-success");
                    AvailableActions deny = new AvailableActions("Deny", "Denial of stage and stops from moving to next stage.", "Deny", "btn-danger");
                    AvailableActions reassign = new AvailableActions("Reassign", "Reassign request for approval to another user.", "Reassign", "btn-info");
                    AvailableActions previous = new AvailableActions("Previous", "Push back to previous stage. This might be used if something was missing is needs updating.", "Previous", "btn-info");


                    List<AvailableActions> actions = new List<AvailableActions>();

                    actions.Add(deny);
                    actions.Add(previous);
                    actions.Add(approval);
                    workflowEntire.Actions = actions;
                }
                else
                {
                    // is first stage and previous is not allowed
                    AvailableActions approval = new AvailableActions("Approval", "Approval of stage and moves to next stage.", "Approval", "btn-success");
                    AvailableActions deny = new AvailableActions("Deny", "Denial of stage and stops from moving to next stage.", "Deny", "btn-danger");
                    AvailableActions reassign = new AvailableActions("Reassign", "Reassign request for approval to another user.", "Reassign", "btn-info");
                    List<AvailableActions> actions = new List<AvailableActions>();
                    actions.Add(deny);
                    actions.Add(approval);
                    workflowEntire.Actions = actions;
                }



            }

            return workflowEntire;
        }
    }
}