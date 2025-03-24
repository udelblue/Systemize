using Microsoft.AspNetCore.Mvc;

namespace Systemize.Controllers
{
    public class ProcessController : Controller
    {
        // GET: ProcessController
        public ActionResult Index()
        {
            return View();
        }



        // GET: ProcessController/Process/5
        public ActionResult Process(int id)
        {
            return View();
        }

        // POST: ProcessController/Process/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Process(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }




    }
}
