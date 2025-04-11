using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel.Dashboard;


namespace Systemize.Controllers
{
    public class DashboardController : Controller
    {


        private readonly SystemizeContext _context;

        public DashboardController(SystemizeContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {

            DashboardEntire dashboardEntire = new DashboardEntire();
            List<Workflow> myassigned = new List<Workflow>();

            var wf1 = await _context.Workflows.FirstOrDefaultAsync(m => m.Id == 10);
            myassigned.Add(wf1);
            var wf2 = await _context.Workflows.FirstOrDefaultAsync(m => m.Id == 1030);
            myassigned.Add(wf2);

            dashboardEntire.myAssigned = myassigned;

            List<Workflow> mywatchlist = new List<Workflow>();
            var wf3 = await _context.Workflows.FirstOrDefaultAsync(m => m.Id == 1011);
            mywatchlist.Add(wf3);

            dashboardEntire.myWatchList = mywatchlist;

            return View(dashboardEntire);
        }

        public async Task<IActionResult> Metrics(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(s => s.Stages)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            MetricsEntire metricsEntire = new MetricsEntire();
            List<MetricsRow> metrics = new List<MetricsRow>();

            Random random = new Random();
            if (workflow.Stages != null && !workflow.Stages.IsNullOrEmpty())
            {
                foreach (var stage in workflow.Stages)
                {

                    var randval = random.Next(0, 101);
                    MetricsRow rw1 = new MetricsRow() { StageName = stage.Name, StageValue = randval };
                    metrics.Add(rw1);
                }
            }


            metricsEntire.Rows = metrics;
            metricsEntire.workflowName = workflow.Name;

            return View(metricsEntire);
        }





        public async Task<IActionResult> TimeLine(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.History)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }


            return View(workflow);
        }



        private string getCurrentUser()
        {
            string email = "System";

            if (this.User != null)
            {
                if (this.User.Identity != null)
                {
                    if (this.User.Identity.IsAuthenticated)
                    {
                        email = this.User.Identity.Name;
                    }
                }
            }

            return email;
        }

    }
}
