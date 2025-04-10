using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

            var wf1 = await _context.Workflows.FirstOrDefaultAsync(m => m.Id == 9);
            myassigned.Add(wf1);
            var wf2 = await _context.Workflows.FirstOrDefaultAsync(m => m.Id == 13);
            myassigned.Add(wf2);


            dashboardEntire.myAssigned = myassigned;



            List<Workflow> mywatchlist = new List<Workflow>();
            var wf = await _context.Workflows.FirstOrDefaultAsync(m => m.Id == 6);
            mywatchlist.Add(wf);

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
                .Include(w => w.History)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            MetricsRow rw1 = new MetricsRow() { StageName = "Test Stage 1", StageValue = 12 };
            MetricsRow rw2 = new MetricsRow() { StageName = "Test Stage 2", StageValue = 22 };

            MetricsEntire metricsEntire = new MetricsEntire();
            List<MetricsRow> metrics = new List<MetricsRow>();
            metrics.Add(rw1);
            metrics.Add(rw2);

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
