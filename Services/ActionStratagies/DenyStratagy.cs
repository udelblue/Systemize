using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel;

namespace Systemize.Services.ActionStratagies
{
    public class DenyStratagy : IActionProcess
    {

        private readonly SystemizeContext _context;


        public DenyStratagy(SystemizeContext context)
        {
            _context = context;
        }


        public Workflow Execute(Workflow workflow, ActionResponse response)
        {
            int count = workflow.Stages.Count();

            // if current stage is null, then set it to the first stage
            if (workflow.CurrentStageId == null & String.IsNullOrEmpty(workflow.Status))
            {
                workflow.CurrentStageId = workflow.Stages[0].Id;
                workflow.Stages[0].StageStatus = "Denied";
                workflow.Status = "Denied";


                History starthistory = new History(response.Executor, response.ActionType, "Workflow Denied");
                workflow.History.Add(starthistory);
                _context.Workflows.Update(workflow);
                _context.SaveChanges();

            }
            else
            {
                int current_index = workflow.Stages.FindIndex(s => s.Id == workflow.CurrentStageId);
                var currentStage = workflow.Stages.Find(s => s.Id == workflow.CurrentStageId);

                // mark current stage as completed
                workflow.Stages[current_index].StageStatus = "Denied";

                workflow.Status = "Denied";

                History starthistory = new History(response.Executor, response.ActionType, "Workflow Denied");
                workflow.History.Add(starthistory);
                _context.Workflows.Update(workflow);
                _context.SaveChanges();
            }

            return workflow;
        }
    }

}

