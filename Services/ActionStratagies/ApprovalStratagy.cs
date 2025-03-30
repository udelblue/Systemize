using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;

namespace Systemize.Services.ActionStratagies
{
    public class ApprovalStratagy : IActionProcess
    {

        private readonly SystemizeContext _context;


        public ApprovalStratagy(SystemizeContext context)
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
                workflow.CurrentStageName = workflow.Stages[0].Name;
                workflow.Stages[0].StageStatus = "Current";
                workflow.Status = "In Progress";


                History starthistory = new History(response.Executor, response.ActionType, "Workflow Started");
                workflow.History.Add(starthistory);


                //assign to currently assign
                Stage firstStage = workflow.Stages[0];
                workflow.CurrentlyAssigned = firstStage.AssignedTo;
                workflow.CurrentStageName = firstStage.Name;

                History firststagehistory = new History(response.Executor, response.ActionType, firstStage.Id, firstStage.Name + " Started");
                workflow.History.Add(firststagehistory);



                _context.Workflows.Update(workflow)



                    ;
                _context.SaveChanges();

            }
            else
            {
                int current_index = workflow.Stages.FindIndex(s => s.Id == workflow.CurrentStageId);
                var currentStage = workflow.Stages.Find(s => s.Id == workflow.CurrentStageId);

                // mark current stage as completed
                workflow.Stages[current_index].StageStatus = "Completed";




                // if current stage is the last stage, then return
                if (current_index == count - 1)
                {

                    //Last stage
                    workflow.CurrentStageId = null;
                    workflow.CurrentStageName = "";
                    workflow.Status = "Completed";
                    workflow.PercentageComplete = 100;
                    History completedhistory = new History(response.Executor, response.ActionType, "Workflow Completed");
                    workflow.History.Add(completedhistory);
                    _context.Workflows.Update(workflow);
                    _context.SaveChanges();


                }
                else
                {


                    workflow.Stages[current_index].StageStatus = "Completed";
                    workflow.Stages[current_index + 1].StageStatus = "Current";

                    var next_index = (current_index + 1);

                    _context.Stages.Update(workflow.Stages[current_index]);
                    _context.Stages.Update(workflow.Stages[next_index]);

                    var nextStage = workflow.Stages[next_index];
                    //assign to currently assign
                    workflow.CurrentlyAssigned = nextStage.AssignedTo;
                    History finishedthistory = new History(response.Executor, response.ActionType, currentStage.Name + " Completed");
                    workflow.History.Add(finishedthistory);





                    //mark next stage as current
                    workflow.CurrentStageId = nextStage.Id;
                    workflow.CurrentStageName = nextStage.Name;






                    //update percentage
                    double percentage = (double)next_index / count;
                    var percent = percentage * 100;
                    workflow.PercentageComplete = (int)percent;


                    //update history
                    History starthistory = new History(response.Executor, response.ActionType, nextStage.Name + " Started");
                    workflow.History.Add(starthistory);
                    _context.Workflows.Update(workflow);
                    _context.SaveChanges();
                }
            }

            return workflow;
        }
    }

}

