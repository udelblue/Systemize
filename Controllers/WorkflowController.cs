using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Systemize.Data;
using Systemize.Models;
using Systemize.Models.ViewModel.Workflow;
using Systemize.Services;

namespace Systemize.Controllers
{
    public class WorkflowController : Controller
    {
        private readonly SystemizeContext _context;

        public WorkflowController(SystemizeContext context)
        {
            _context = context;
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
                ViewData["WorkflowId"] = id;
            }

            return View();

        }


        // Replace the existing method with the following code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StageAdd(int? id, string? test)
        {
            var workflow = await _context.Workflows
                .Include(w => w.Stages)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workflow == null)
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
                WorkflowId = workflow.Id
            };

            _context.Stages.Add(stage);
            await _context.SaveChangesAsync(); // This should now work with the correct using directive

            return RedirectToAction(nameof(Details), new { id = id });
        }



        // WORKFLOW


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

            if (TempData["Action_Message"] != null)
            {
                ViewData["Action_Message"] = TempData["Action_Message"].ToString();
            }

            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.Stages)
                .Include(w => w.Documents)
                .Include(w => w.Tags)
                .Include(w => w.Links)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }



            WorkflowEntire workflowEntire = new WorkflowEntire();


            workflowEntire.isReadonly = workflow.Status != null && workflow.Status.ToLower() == "completed" | workflow.Status.ToLower() == "denied" | workflow.Status.ToLower() == "cancelled" ? true : false;



            //workflow not started
            if (workflow.Status != null && workflow.Status.ToLower() != "completed" && workflow.CurrentStageId == null && workflow.Stages != null && workflow.Stages.Count > 0)
            {
                AvailableActions cancel = new AvailableActions("Cancel", "Cancel workflow.", "Cancel", "btn-warning");
                AvailableActions start = new AvailableActions("Start", "Start the workflow to begin at first stage.", "Start", "btn-success");
                List<AvailableActions> actions = new List<AvailableActions>();

                actions.Add(cancel);
                actions.Add(start);

                workflowEntire.Actions = actions;
            }
            //workflow already started
            else if (workflow.Status != null && workflow.Status.ToLower() != "completed" && workflow.Status.ToLower() != "denied" && workflow.Status.ToLower() != "draft" && workflow.Status.ToLower() != "cancelled")
            {

                AvailableActions approval = new AvailableActions("Approval", "Approval of stage and moves to next stage.", "Approval", "btn-success");
                AvailableActions deny = new AvailableActions("Deny", "Denial of stage and stops from moving to next stage.", "Deny", "btn-danger");
                AvailableActions reassign = new AvailableActions("Reassign", "Reassign request for approval to another user.", "Reassign", "btn-info");


                List<AvailableActions> actions = new List<AvailableActions>();

                actions.Add(deny);
                actions.Add(approval);
                workflowEntire.Actions = actions;
            }


            workflowEntire.Workflow = workflow;


            return View(workflowEntire);
        }


        // GET: Workflow/Meta/5
        public async Task<IActionResult> Meta(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //load workflow with eager loading
            var workflow = await _context.Workflows
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            return View(workflow);
        }


        // GET: Workflow/stages/5
        public async Task<IActionResult> Stages(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.Stages)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            return View(workflow);
        }



        // GET: Workflow/History/5
        public async Task<IActionResult> History(int? id)
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
        public async Task<IActionResult> Create([Bind("Name,Description")] Workflow workflow)
        {
            //TODO get creator and replace TBD
            // History starthistory = new History("TBD", "Created", "Workflow Created");
            // workflow.History.Add(starthistory);


            workflow.CreatedOn = DateTime.Now;
            workflow.Status = "Draft";
            _context.Add(workflow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

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
            WorkflowEdit we = new WorkflowEdit() { Id = workflow.Id, Name = workflow.Name, Description = workflow.Description };

            return View(we);
        }

        // POST: Workflow/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, Name,Description")] WorkflowEdit workflowedit)
        {
            //workflowedit.Id = id;


            var workflow = await _context.Workflows

               .FirstOrDefaultAsync(m => m.Id == id);

            if (ModelState.IsValid)
            {



                try
                {



                    if (workflow == null)
                    {
                        return NotFound();
                    }
                    workflow.Name = workflowedit.Name;
                    workflow.Description = workflowedit.Description;


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
            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.Stages)
                .Include(w => w.Documents)
                .Include(w => w.Tags)
                .Include(w => w.Links)
                .Include(w => w.History)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow != null)
            {
                _context.Workflows.Remove(workflow);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // DOCUMENTS 


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
            //return PartialView()
        }


        // Post: Workflow/Upload/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(int? id, string? test)
        {

            var email = "System";

            var workflow = await _context.Workflows
            .Include(w => w.Documents)
            .Include(w => w.History)
            .FirstOrDefaultAsync(m => m.Id == id)
            ;
            if (workflow == null)
            {
                return NotFound();
            }


            var workflowId = HttpContext.Request.Form["WorkflowId"];
            var document_type = HttpContext.Request.Form["DocumentType"];
            var document_description = HttpContext.Request.Form["Description"];

            if (String.IsNullOrEmpty(document_type))
            {
                document_type = "Other";
            }


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
                                Title = formFile.FileName,
                                Description = document_description,
                                DocumentType = document_type,
                                Content = memoryStream.ToArray(),
                                ContentType = formFile.ContentType
                            };

                            workflow.Documents.Add(document);
                            var currentStageID = workflow.CurrentStageId ?? null;
                            var currentStageName = workflow.CurrentStageName ?? null;
                            History firststagehistory = new History(email, "", currentStageID, currentStageName, "Minor", "Document Uploaded", "File:" + formFile.FileName);
                            workflow.History.Add(firststagehistory);
                        }
                    }
                }

                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Details), new { id = workflowId });
        }

        // GET: Workflow/DocumentEdit/[id]?documentId=[document]
        //[Route("Workflow/{id:int}/Document/Edit/{document:int}")]

        [HttpGet]
        public async Task<IActionResult> DocumentEdit(int? id, int? document)
        {
            if (id == null || document == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows
                .Include(w => w.Documents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workflow == null)
            {
                return NotFound();
            }

            var local_document = workflow.Documents.FirstOrDefault(d => d.DocumentID == document);
            if (local_document == null)
            {
                return NotFound();
            }

            DocumentEdit de = new DocumentEdit()
            {
                Title = local_document.Title,
                ContentType = local_document.ContentType,
                Description = local_document.Description,
                DocumentType = local_document.ContentType,
                DocumentID = local_document.DocumentID
            };




            ViewData["WorkflowId"] = id;

            return View(de);
        }


        // POST: Workflow/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DocumentEdit(int id, [Bind("DocumentID,Title,Description,DocumentType,ContentType")] DocumentEdit document)
        {
            var email = "System";


            var workflow = await _context.Workflows
               .Include(w => w.Documents)
               .Include(w => w.History)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (ModelState.IsValid)
            {

                try
                {
                    if (workflow == null)
                    {
                        return NotFound();
                    }
                    Document local_doc = workflow.Documents.FirstOrDefault(w => w.DocumentID == document.DocumentID);
                    local_doc.Title = document.Title;
                    local_doc.Description = document.Description;
                    local_doc.ContentType = document.ContentType;
                    local_doc.DocumentType = document.DocumentType;


                    History firststagehistory = new History(email, "", workflow.CurrentStageId, workflow.CurrentStageName, "Minor", "Document Edit", "");
                    workflow.History.Add(firststagehistory);



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


        // GET: Workflow/DocumentDelete/[id]?documentId=[documentId]

        public async Task<IActionResult> DocumentDelete(int? id, int? document)
        {
            if (id == null || document == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows
                .Include(w => w.Documents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workflow == null)
            {
                return NotFound();
            }

            var local_document = workflow.Documents.FirstOrDefault(d => d.DocumentID == document);
            if (local_document == null)
            {
                return NotFound();
            }

            return View(local_document);

        }




        // GET: Workflow/DocumentView/[id]?documentId=[documentId]

        public async Task<IActionResult> DocumentView(int? id, int? document)
        {
            if (id == null || document == null)
            {
                return NotFound();
            }

            ViewBag.Id = id.ToString();
            ViewBag.DocumentId = document.ToString();

            return View();
        }



        public async Task<IActionResult> DocumentDownload(int? id, int? document)
        {
            if (id == null || document == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows
                .Include(w => w.Documents)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workflow == null)
            {
                return NotFound();
            }

            var local_document = workflow.Documents.FirstOrDefault(d => d.DocumentID == document);
            if (local_document == null)
            {
                return NotFound();
            }


            string fileType = local_document.ContentType;
            string fileName = local_document.Title;
            MemoryStream stream = new MemoryStream(local_document.Content);


            stream.Position = 0;

            return File(stream, fileType, fileName);


        }




        [HttpPost, ActionName("DocumentDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DocumentDeleteConfirmed(int? id, int? documentID)
        {
            var email = "System";

            if (id == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows
                .Include(w => w.Documents)
                .Include(w => w.History)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workflow == null)
            {
                return NotFound();
            }

            var local_document = workflow.Documents.FirstOrDefault(d => d.DocumentID == documentID);
            if (local_document != null)
            {
                string filename = local_document.Title;
                workflow.Documents.Remove(local_document);
                var currentStageID = workflow.CurrentStageId ?? null;
                var currentStageName = workflow.CurrentStageName ?? null;
                History firststagehistory = new History(email, "", currentStageID, currentStageName, "Minor", "Document Removed", "File:" + filename);
                workflow.History.Add(firststagehistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        // LINKS

        // GET: Workflow/LinkAdd/id
        [HttpGet]
        public IActionResult LinkAdd(int? id)
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


        // Post: Workflow/LinkAdd/id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkAdd(int? id, string? test)
        {

            var email = "System";
            var workflow = await _context.Workflows
            .Include(w => w.Links)
            .Include(w => w.History)
            .FirstOrDefaultAsync(m => m.Id == id)
            ;
            if (workflow == null)
            {
                return NotFound();
            }



            var title = HttpContext.Request.Form["Title"];
            var URL = HttpContext.Request.Form["URL"];
            Link link = new Link() { Title = title, URL = URL };
            workflow.Links.Add(link);
            var currentStageID = workflow.CurrentStageId ?? null;
            var currentStageName = workflow.CurrentStageName ?? null;

            History firststagehistory = new History(email, "", currentStageID, currentStageName, "Minor", "Link Added", "URL:" + link.URL);
            workflow.History.Add(firststagehistory);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = id });
        }


        // GET: Workflow/LinkDelete/[id]?linktId=[LinkId]

        public async Task<IActionResult> LinkDelete(int? id, int? link)
        {




            if (id == null || link == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows
                .Include(w => w.Links)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workflow == null)
            {
                return NotFound();
            }

            var local_link = workflow.Links.FirstOrDefault(d => d.LinkID == link);
            if (local_link == null)
            {
                return NotFound();
            }

            return View(local_link);

        }



        [HttpPost, ActionName("LinkDelete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkDeleteConfirmed(int? id, int? linkID)
        {
            var email = "System";

            if (id == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows
                .Include(w => w.Links)
                .Include(w => w.History)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (workflow == null)
            {
                return NotFound();
            }

            var local_document = workflow.Links.FirstOrDefault(d => d.LinkID == linkID);
            if (local_document != null)
            {
                var json = local_document.ToJson();
                workflow.Links.Remove(local_document);
                var currentStageID = workflow.CurrentStageId ?? null;
                var currentStageName = workflow.CurrentStageName ?? null;
                History firststagehistory = new History(email, "", currentStageID, currentStageName, "Minor", "Link Removed", "JSON:" + json);
                workflow.History.Add(firststagehistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // GET: Workflow/LinkEdit/[id]?linkId=[link]

        [HttpGet]
        public async Task<IActionResult> LinkEdit(int? id, int? link)
        {
            if (id == null || link == null)
            {
                return NotFound();
            }

            var workflow = await _context.Workflows
              .Include(w => w.Links)
              .Include(w => w.History)
              .FirstOrDefaultAsync(m => m.Id == id);

            if (workflow == null)
            {
                return NotFound();
            }

            var local_link = workflow.Links.FirstOrDefault(d => d.LinkID == link);
            if (local_link == null)
            {
                return NotFound();
            }

            return View(local_link);
        }









        // POST: Workflow/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LinkEdit(int id, [Bind("LinkID, Title,Description, URL")] Link link)
        {
            var email = "System";


            var workflow = await _context.Workflows
               .Include(w => w.Links)
               .Include(w => w.History)
               .FirstOrDefaultAsync(m => m.Id == id);

            if (ModelState.IsValid)
            {

                try
                {
                    if (workflow == null)
                    {
                        return NotFound();
                    }
                    Link local_link = workflow.Links.FirstOrDefault(w => w.LinkID == link.LinkID);
                    local_link.Title = link.Title;
                    local_link.Description = link.Description;
                    local_link.URL = link.URL;


                    History firststagehistory = new History(email, "", workflow.CurrentStageId, workflow.CurrentStageName, "Minor", "Link Updated", "JSON:" + link.ToJson());
                    workflow.History.Add(firststagehistory);



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


        // Post: Workflow/TagAdd/id

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TagAdd(int? id)
        {
            var workflow = await _context.Workflows
            .Include(w => w.Tags)
            .FirstOrDefaultAsync(m => m.Id == id)
            ;
            if (workflow == null)
            {
                return NotFound();
            }

            var name = HttpContext.Request.Form["Name"];

            WorkflowTag tag = new WorkflowTag() { Name = name };
            workflow.Tags.Add(tag);

            await _context.SaveChangesAsync();

            return Ok(workflow.Tags);
        }



        // util methods
        private bool WorkflowExists(int id)
        {
            return _context.Workflows.Any(e => e.Id == id);
        }





        //ACTION

        // GET: Workflow/Action/[id]?action=[action]
        public async Task<IActionResult> Action(int? id, string? act)
        {
            var workflow = await _context.Workflows
          .Include(w => w.Stages)
          .Include(w => w.Documents)
          .Include(w => w.Tags)
          .Include(w => w.History)
          .Include(w => w.Links)
          .FirstOrDefaultAsync(m => m.Id == id)
          ;
            if (workflow == null)
            {
                return NotFound();
            }

            ViewBag.Act = act;
            ViewBag.Id = id;

            ActionResponse actionResponse = new ActionResponse();
            actionResponse.ActionType = act;
            actionResponse.Executor = "System";

            // engine picks stratagy
            Engine engine = new Engine(_context, workflow);
            workflow = engine.Process(actionResponse);

            TempData["Action_Message"] = "The " + act + " action has been completed.";
            return RedirectToAction(nameof(Details), new { id = id });

        }



    }
}



