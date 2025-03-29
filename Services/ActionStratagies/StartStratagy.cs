using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel;

namespace Systemize.Services.ActionStratagies
{
    public class StartStratagy : IActionProcess
    {
        private readonly SystemizeContext _context;

        public StartStratagy(SystemizeContext context)
        {
            _context = context;
        }

        public Workflow Execute(Workflow workflow, ActionResponse response)
        {
            int count = workflow.Stages.Count();

            // if current stage is null, then set it to the first stage
            if (workflow.CurrentStageId == null)
            {
                workflow.CurrentStageId = workflow.Stages[0].Id;
                workflow.CurrentStageName = workflow.Stages[0].Name;
                workflow.PercentageComplete = 0;
                workflow.Stages[0].StageStatus = "Current";
                workflow.Status = "In Progress";

                //assign to currently assign
                workflow.CurrentlyAssigned = workflow.Stages[0].AssignedTo;

                History starthistory = new History(response.Executor, response.ActionType, "Workflow Started");
                workflow.History.Add(starthistory);
                _context.Workflows.Update(workflow);
                _context.SaveChanges();

            }


            return workflow;
        }


    }
}
