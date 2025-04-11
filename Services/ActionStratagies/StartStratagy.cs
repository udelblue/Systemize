using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;

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
                Stage firststage = workflow.Stages[0];


                workflow.CurrentStageId = firststage.Id;
                workflow.CurrentStageName = firststage.Name;
                workflow.PercentageComplete = 0;
                if (workflow.AssignedTo == null)
                {
                    workflow.AssignedTo = new List<String>();
                }
                else
                {
                    workflow.AssignedTo.Clear();
                }


                workflow.AssignedTo.AddRange(firststage.AssignedTo);
                workflow.Stages[0].StageStatus = "Current";
                workflow.Status = "In Progress";

                //assign to currently assign
                // TODO fix workflow.AssignedTo.Add(firststage.AssignedTo);


                History starthistory = new History(response.Executor, response.ActionType, null, null, "Major", "Workflow Started", "");
                workflow.History.Add(starthistory);



                History stageHistory = new History(response.Executor, response.ActionType, firststage.Id, firststage.Name, "Major", "Stage Started", "");
                workflow.History.Add(stageHistory);

                _context.Workflows.Update(workflow);
                _context.SaveChanges();

            }


            return workflow;
        }


    }
}
