using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel;
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
                case "approve":
                    Console.WriteLine("approve");
                    process = new ApprovalStratagy(_context);
                    _workflow = process.Execute(_workflow, actionRespone);
                    break;
                case "deny":
                    Console.WriteLine("deny");
                    process = new DenyStratagy(_context);
                    _workflow = process.Execute(_workflow, actionRespone);
                    break;

                default:
                    Console.WriteLine("Invalid Action");
                    throw new Exception("Invalid action requested from Engine. ");
                    break;
            }

            return _workflow;





        }





    }
}
