using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Systemize.Data;
using Systemize.Models;

namespace Systemize.Controllers
{
    public class WorkflowTemplateController : Controller
    {
        private readonly SystemizeContext _context;

        public WorkflowTemplateController(SystemizeContext context)
        {
            _context = context;
        }

        // GET: WorkflowTemplate
        public async Task<IActionResult> Index()
        {
            if (TempData["Message"] != null)
            {

                ViewBag.Message = TempData["Message"];

            }


            return View(await _context.WorkflowTemplate.Include(w => w.Stages).ToListAsync());
        }

        // GET: WorkflowTemplate/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflowTemplate = await _context.WorkflowTemplate
                .Include(w => w.Stages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workflowTemplate == null)
            {
                return NotFound();
            }

            return View(workflowTemplate);
        }

        // GET: WorkflowTemplate/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WorkflowTemplate/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] WorkflowTemplate workflowTemplate)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workflowTemplate);
                _context.SaveChanges();
                var tmpID = workflowTemplate.Id;
                return RedirectToAction(nameof(StageAdd), new { id = tmpID });

                // return RedirectToAction(nameof(Index));
            }
            return View(workflowTemplate);
        }

        // GET: WorkflowTemplate/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflowTemplate = await _context.WorkflowTemplate.FindAsync(id);
            if (workflowTemplate == null)
            {
                return NotFound();
            }
            return View(workflowTemplate);
        }

        // POST: WorkflowTemplate/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] WorkflowTemplate workflowTemplate)
        {
            if (id != workflowTemplate.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workflowTemplate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkflowTemplateExists(workflowTemplate.Id))
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
            return View(workflowTemplate);
        }

        // GET: WorkflowTemplate/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflowTemplate = await _context.WorkflowTemplate
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workflowTemplate == null)
            {
                return NotFound();
            }

            return View(workflowTemplate);
        }

        // POST: WorkflowTemplate/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workflowTemplate = await _context.WorkflowTemplate.FindAsync(id);
            if (workflowTemplate != null)
            {
                _context.WorkflowTemplate.Remove(workflowTemplate);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // GET: WorkflowTemplate/CreateWorkflowFromTemplate/5
        public async Task<IActionResult> CreateWorkflowFromTemplate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflowTemplate = await _context.WorkflowTemplate
                .Include(w => w.Stages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workflowTemplate == null)
            {
                return NotFound();
            }

            // Deep clone the workflowTemplate
            var newWorkflow = new Workflow
            {
                Name = workflowTemplate.Name + " - Copy",
                Description = workflowTemplate.Description,
                Stages = workflowTemplate.Stages?.Select(s => new Stage
                {
                    Name = s.Name,
                    Description = s.Description,
                    StageType = s.StageType,
                    Properties = s.Properties
                }).ToList()
            };

            newWorkflow.CreatedOn = DateTime.Now;
            newWorkflow.Status = "Draft";

            _context.Workflows.Add(newWorkflow);
            await _context.SaveChangesAsync();


            return RedirectToAction("Index", "Workflow");
            //return RedirectToAction(nameof(Details), new { id = newWorkflow.Id });
        }

        // GET: WorkflowTemplate/CreateWorkflowFromTemplate/5
        public async Task<IActionResult> CreateWorkflowFromTemplateAndStart(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflowTemplate = await _context.WorkflowTemplate
                .Include(w => w.Stages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workflowTemplate == null)
            {
                return NotFound();
            }


            if (workflowTemplate.Stages.Count == 0)
            {
                TempData["Message"] = "Workflow has zero stages. Workflow must have one or more stages.";
                return RedirectToAction(nameof(Index));

            }


            // Deep clone the workflowTemplate
            var newWorkflow = new Workflow
            {
                Name = workflowTemplate.Name + "",
                Description = workflowTemplate.Description,
                Stages = workflowTemplate.Stages?.Select(s => new Stage
                {
                    Name = s.Name,
                    Description = s.Description,
                    StageType = s.StageType,
                    Properties = s.Properties
                }).ToList()
            };

            newWorkflow.CreatedOn = DateTime.Now;
            newWorkflow.Status = "Draft";

            _context.Workflows.Add(newWorkflow);
            await _context.SaveChangesAsync();

            var wid = newWorkflow.Id.ToString();
            //redirect to start workflow
            //Workflow / Action / [id] ? act = [action]
            return Redirect("/Workflow/Action/" + wid + "?act=Start");


        }





        // GET: WorkflowTemplate/Duplicate/5
        public async Task<IActionResult> Duplicate(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workflowTemplate = await _context.WorkflowTemplate
                .Include(w => w.Stages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workflowTemplate == null)
            {
                return NotFound();
            }

            // Deep clone the workflowTemplate
            var newWorkflowTemplate = new WorkflowTemplate
            {
                Name = workflowTemplate.Name + " - Copy",
                Description = workflowTemplate.Description,
                Stages = workflowTemplate.Stages?.Select(s => new Stage
                {
                    Name = s.Name,
                    Description = s.Description,
                    StageType = s.StageType,
                    Properties = s.Properties
                }).ToList()
            };

            _context.WorkflowTemplate.Add(newWorkflowTemplate);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = newWorkflowTemplate.Id });
        }




        // STAGE
        // GET: Workflow/StageAdd/id
        [HttpGet]
        public IActionResult StageAdd(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            else
            {
                ViewData["WorkflowTemplateId"] = id;
            }

            return View();

        }


        // Post: Workflow/StageAdd/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StageAdd(int? id, string? test)
        {
            var workflowtemplate = await _context.WorkflowTemplate
            .Include(w => w.Stages)
            .FirstOrDefaultAsync(m => m.Id == id)
            ;
            if (workflowtemplate == null)
            {
                return NotFound();
            }



            var name = HttpContext.Request.Form["Name"];
            var description = HttpContext.Request.Form["Description"];
            var stagetype = HttpContext.Request.Form["StageType"];

            Stage stage = new Stage()
            {
                Name = name,
                Description = description,
                StageType = stagetype,
                Properties = "{}",
                WorkflowTemplateId = workflowtemplate.Id

            };

            _context.Stages.Add(stage);


            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }




        private bool WorkflowTemplateExists(int id)
        {
            return _context.WorkflowTemplate.Any(e => e.Id == id);
        }
    }
}
