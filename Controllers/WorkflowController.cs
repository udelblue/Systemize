using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Systemize.Data;
using Systemize.Models;

namespace Systemize.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly SystemizeContext _context;

        public WorkflowController(SystemizeContext context)
        {
            _context = context;
        }

        // GET: Workflow
        public async Task<IActionResult> Index()
        {
            return View(await _context.Workflows.ToListAsync());
        }

        // GET: Workflow/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.Stages)
                .Include(w => w.History)
                .Include(w => w.Documents)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            return View(workflow);
        }



        // GET: Workflow/Upload/id
        [HttpGet]
        public IActionResult Upload(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                ViewData["WorkflowId"] = id;
            }

            return View();
        }


        // Post: Workflow/Upload/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int? id, string? test)
        {
            var workflow = await _context.Workflows
            .Include(w => w.Documents)
            .FirstOrDefaultAsync(m => m.Id == id)
            ;
            if (workflow == null)
            {
                return NotFound();
            }


            var workflowId = HttpContext.Request.Form["WorkflowId"];
            var document_name = HttpContext.Request.Form["Name"];
            var document_description = HttpContext.Request.Form["Description"];



            IFormFileCollection files = HttpContext.Request.Form.Files;
            if (files.Count != 0)
            {
                foreach (var formFile in files)
                {
                    if (formFile.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await formFile.CopyToAsync(memoryStream);
                            var document = new Document
                            {
                                Title = document_name,
                                Description = document_description,
                                Content = memoryStream.ToArray(),
                                ContentType = formFile.ContentType
                            };

                            workflow.Documents.Add(document);

                        }
                    }
                }

                await _context.SaveChangesAsync();
            }



            return RedirectToAction(nameof(Details), new { id = workflowId });
        }



        // GET: Workflow/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workflow/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,CurrentStageId")] Workflow workflow)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workflow);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(workflow);
        }

        // GET: Workflow/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows.FindAsync(id);
            if (workflow == null)
            {
                return NotFound();
            }
            return View(workflow);
        }

        // POST: Workflow/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CurrentStageId")] Workflow workflow)
        {
            if (id != workflow.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workflow);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkflowExists(workflow.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workflow);
        }

        // GET: Workflow/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workflow == null)
            {
                return NotFound();
            }

            return View(workflow);
        }

        // POST: Workflow/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workflow = await _context.Workflows.FindAsync(id);
            if (workflow != null)
            {
                _context.Workflows.Remove(workflow);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkflowExists(int id)
        {
            return _context.Workflows.Any(e => e.Id == id);
        }
    }
}
