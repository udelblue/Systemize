﻿using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;

namespace Systemize.Services.ActionStratagies
{
    public class CancelStratagy : IActionProcess
    {

        private readonly SystemizeContext _context;


        public CancelStratagy(SystemizeContext context)
        {
            _context = context;
        }


        public Workflow Execute(Workflow workflow, ActionResponse response)
        {
            int count = workflow.Stages.Count();

            // if current stage is null, then set it to the first stage
            if (workflow.CurrentStageId == null)
            {
                if (workflow.Stages.Count > 0)
                {
                    if (workflow.AssignedTo == null)
                    {
                        workflow.AssignedTo = new List<String>();
                    }
                    else
                    {
                        workflow.AssignedTo.Clear();
                    }
                    workflow.CurrentStageId = workflow.Stages[0].Id;
                    workflow.Stages[0].StageStatus = "Cancelled";
                    workflow.Status = "Cancelled";


                    History starthistory = new History(response.Executor, response.ActionType, workflow.Stages[0].Id, workflow.Stages[0].Name, "Major", "Workflow Cancelled", "");
                    workflow.History.Add(starthistory);
                    _context.Stages.Update(workflow.Stages[0]);
                    _context.Workflows.Update(workflow);
                    _context.SaveChanges();
                }
                else
                {
                    workflow.Status = "Cancelled";

                    History starthistory = new History(response.Executor, response.ActionType, null, null, "Major", "Workflow Cancelled", "");
                    workflow.History.Add(starthistory);
                    _context.Workflows.Update(workflow);
                    _context.SaveChanges();
                }


            }
            else
            {
                int current_index = workflow.Stages.FindIndex(s => s.Id == workflow.CurrentStageId);
                var currentStage = workflow.Stages.Find(s => s.Id == workflow.CurrentStageId);


                // mark current stage as completed
                currentStage.StageStatus = "Cancelled";
                _context.Stages.Update(currentStage);


                workflow.Status = "Cancelled";

                History starthistory = new History(response.Executor, response.ActionType, currentStage.Id, currentStage.Name, "Major", "Workflow Cancelled", "");
                workflow.History.Add(starthistory);
                _context.Stages.Update(currentStage);
                _context.Workflows.Update(workflow);
                _context.SaveChanges();
            }

            return workflow;
        }
    }

}

