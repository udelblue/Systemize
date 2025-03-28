using Microsoft.AspNetCore.Mvc;

namespace Systemize.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
