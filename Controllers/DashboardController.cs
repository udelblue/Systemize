using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Systemize.Data;

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

            return View(workflow);
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
