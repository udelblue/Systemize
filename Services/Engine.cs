﻿using Systemize.Data;
using Systemize.Models;

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




        public Workflow Process(string action, string executor)
        {
            int count = _workflow.Stages.Count();

            // if current stage is null, then set it to the first stage
            if (_workflow.CurrentStageId == null & String.IsNullOrEmpty(_workflow.Status))
            {
                _workflow.CurrentStageId = _workflow.Stages[0].Id;
                _workflow.Stages[0].StageStatus = "Current";
                _workflow.Status = "In Progress";

                //assign to currently assign
                _workflow.CurrentlyAssigned = _workflow.Stages[0].AssignedTo;

                History starthistory = new History(executor, action, "Workflow Started");
                _workflow.History.Add(starthistory);
                _context.SaveChangesAsync();

            }
            else
            {
                int current_index = _workflow.Stages.FindIndex(s => s.Id == _workflow.CurrentStageId);
                var currentStage = _workflow.Stages.Find(s => s.Id == _workflow.CurrentStageId);

                // mark current stage as completed
                _workflow.Stages[current_index].StageStatus = "Completed";




                // if current stage is the last stage, then return
                if (current_index == count - 1)
                {


                    //Last stage
                    _workflow.CurrentStageId = null;
                    _workflow.Status = "Completed";
                    History completedhistory = new History(executor, action, "Workflow Completed");
                    _workflow.History.Add(completedhistory);
                    _context.SaveChangesAsync();


                }
                else
                {





                    var nextStage = _workflow.Stages[current_index + 1];
                    //assign to currently assign
                    _workflow.CurrentlyAssigned = nextStage.AssignedTo;
                    History finishedthistory = new History(executor, action, "Stage Completed" + currentStage.Name);
                    _workflow.History.Add(finishedthistory);





                    //mark next stage as current
                    _workflow.Stages[current_index + 1].StageStatus = "Current";
                    _workflow.CurrentStageId = nextStage.Id;
                    History starthistory = new History(executor, action, "Stage Started" + nextStage.Name);
                    _workflow.History.Add(starthistory);

                    _context.SaveChangesAsync();
                }
            }


            return _workflow;





        }





    }
}
