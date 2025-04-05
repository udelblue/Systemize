using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;

namespace Systemize.Services.ActionStratagies
{
    public class PreviousStratagy : IActionProcess
    {

        private readonly SystemizeContext _context;


        public PreviousStratagy(SystemizeContext context)
        {
            _context = context;
        }


        public Workflow Execute(Workflow workflow, ActionResponse response)
        {
            int count = workflow.Stages.Count();

            // if current stage is null, then set it to the first stage
            if (workflow.CurrentStageId == null & String.IsNullOrEmpty(workflow.Status))
            {
                // TODO throw error
            }
            else
            {
                int current_index = workflow.Stages.FindIndex(s => s.Id == workflow.CurrentStageId);
                var currentStage = workflow.Stages.Find(s => s.Id == workflow.CurrentStageId);


                // first stage 
                if (current_index == 0)
                {
                    // TODO throw error
                }
                else
                {

                    workflow.Stages[current_index].StageStatus = "";
                    workflow.Stages[current_index - 1].StageStatus = "Current";

                    var previous_index = (current_index - 1);

                    _context.Stages.Update(workflow.Stages[current_index]);
                    _context.Stages.Update(workflow.Stages[previous_index]);

                    var previousStage = workflow.Stages[previous_index];
                    //assign to currently assign
                    workflow.AssignedTo.Add(previousStage.AssignedTo);

                    //mark next stage as current
                    workflow.CurrentStageId = previousStage.Id;
                    workflow.CurrentStageName = previousStage.Name;

                    //update percentage
                    double percentage = (double)previous_index / count;
                    var percent = percentage * 100;
                    workflow.PercentageComplete = (int)percent;

                    History previoushistory = new History(response.Executor, response.ActionType, previousStage.Id, previousStage.Name, "Major", "Pushed to previous stage", "");
                    workflow.History.Add(previoushistory);

                    //update history
                    History starthistory = new History(response.Executor, response.ActionType, previousStage.Id, previousStage.Name, "Major", "Stage Started", "");
                    workflow.History.Add(starthistory);
                    _context.Workflows.Update(workflow);
                    _context.SaveChanges();
                }
            }

            return workflow;
        }
    }

}

