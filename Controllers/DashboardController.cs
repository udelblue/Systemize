using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Systemize.Data;
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


        public IActionResult Index()
        {
            return View();
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


    }
}
