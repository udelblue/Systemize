using Systemize.Data;
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


        public Workflow Process()
        {
            int count = _workflow.Stages.Count();

            // if current stage is null, then set it to the first stage
            if (_workflow.CurrentStageId == null & String.IsNullOrEmpty(_workflow.Status))
            {
                _workflow.CurrentStageId = _workflow.Stages[0].Id;
                _workflow.Status = "In Progress";
                _context.SaveChangesAsync();

            }
            else
            {
                var currentStage = _workflow.Stages.Find(s => s.Id == _workflow.CurrentStageId);
                int current_index = _workflow.Stages.FindIndex(s => s.Id == _workflow.CurrentStageId);

                // if current stage is the last stage, then return
                if (current_index == count - 1)
                {
                    //Last stage
                    _workflow.CurrentStageId = null;
                    _workflow.Status = "Completed";
                    _context.SaveChangesAsync();

                }
                else
                {
                    var nextStage = _workflow.Stages[current_index + 1];
                    _workflow.CurrentStageId = nextStage.Id;
                    _context.SaveChangesAsync();
                }
            }


            return _workflow;


        }


    }
}
