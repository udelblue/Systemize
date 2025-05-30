﻿using Microsoft.AspNetCore.Authorization;
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




        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChatSubmit(int id, [FromBody] string chat)
        {


            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.WorkflowForm)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            if (!System.String.IsNullOrEmpty(chat))
            {
                Console.Write(chat);
                if (workflow.WorkflowForm == null)
                {
                    workflow.WorkflowForm = new WorkflowForm();
                }

                //TODO process chat




            }


            return Ok("Data Received");
        }




        // GET: Workflow/FormBuilder/5
        [Authorize]
        public async Task<IActionResult> FormBuilder(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            //load workflow with eager loading
            var workflow = await _context.Workflows
                 .Include(w => w.WorkflowForm)
                 .FirstOrDefaultAsync(m => m.Id == id)
                 ;
            if (workflow == null)
            {
                return NotFound();
            }


            return View(workflow);
        }

        // GET: Workflow/FormRender/5
        [Authorize]
        public async Task<IActionResult> FormRender(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }


            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.WorkflowForm)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            if (workflow.WorkflowForm == null)
            {
                workflow.WorkflowForm = new WorkflowForm();
            }

            return View(workflow);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FormBuilderSubmit(int id, [FromBody] string data)
        {


            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.WorkflowForm)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            if (!System.String.IsNullOrEmpty(data))
            {
                Console.Write(data);
                if (workflow.WorkflowForm == null)
                {
                    workflow.WorkflowForm = new WorkflowForm();
                }


                workflow.FormData = data;
                _context.Workflows.Update(workflow);
                _context.SaveChanges();

            }


            return Ok("Data Received");
        }



        [HttpPost]
        [Authorize]
        public async Task<IActionResult> FormRenderSubmit(int id, [FromBody] string data)
        {

            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.WorkflowForm)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            if (!System.String.IsNullOrEmpty(data))
            {
                Console.Write(data);
                if (workflow.WorkflowForm == null)
                {
                    workflow.WorkflowForm = new WorkflowForm();
                }


                workflow.FormData = data;
                _context.Workflows.Update(workflow);
                _context.SaveChanges();

            }


            return Ok("Data Received");
        }



        // STAGE
        // GET: Workflow/StageAdd/id
        [HttpGet]
        [Authorize]
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

            if (TempData["Message"] != null)
            {
                ViewData["Message"] = TempData["Message"].ToString();
            }


            return View();

        }


        // Replace the existing method with the following code
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
            var assignedTo = HttpContext.Request.Form["AssignedTo"];

            if (assignedTo == "")
            {
                TempData["Message"] = "AssignedTo can't be empty";
                return RedirectToAction(nameof(StageAdd), new { id = id });
            }


            Stage stage = new Stage()
            {
                Name = name,
                Description = description,
                StageType = stagetype,
                AssignedTo = assignedTo.ToString().Split(';').ToList(),
                Properties = "{}",
                WorkflowId = workflow.Id
            };

            _context.Stages.Add(stage);
            await _context.SaveChangesAsync(); // This should now work with the correct using directive

            return RedirectToAction(nameof(Details), new { id = id });
        }



        // WORKFLOW


        // GET: Workflow
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (TempData["Message"] != null)
            {

                ViewBag.Message = TempData["Message"];

            }


            return View(await _context.Workflows.ToListAsync());
        }

        // GET: Workflow/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {

            string currentUser = getCurrentUser();


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

            Engine engine = new Engine(_context, workflow);
            WorkflowEntire workflowEntire = engine.AvailableActions(workflow, currentUser);


            workflowEntire.Workflow = workflow;
            return View(workflowEntire);

        }

        // GET: Workflow/Meta/5
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

        // GET: Workflow/Notes/5
        [Authorize]
        public async Task<IActionResult> Notes(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //load workflow with eager loading
            var workflow = await _context.Workflows
                .Include(w => w.Notes)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            return View(workflow);
        }




        // GET: Workflow/Create
        [Authorize]
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

            workflow.CreatedBy.Add(getCurrentUser());
            workflow.AssignedTo = new List<String>();
            workflow.CreatedOn = DateTime.Now;
            workflow.Status = "Draft";
            _context.Add(workflow);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Workflow/Edit/5
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

            if (System.String.IsNullOrEmpty(document_type))
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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


        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> DocumentDeleteConfirmed(int? id, int? documentID)
        {
            var email = getCurrentUser();

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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> LinkAdd(int? id, string? test)
        {

            var email = getCurrentUser();
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> LinkDeleteConfirmed(int? id, int? linkID)
        {
            var email = getCurrentUser();

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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> LinkEdit(int id, [Bind("LinkID, Title,Description, URL")] Link link)
        {
            var email = getCurrentUser();


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
        [Authorize]
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
            if (!System.String.IsNullOrEmpty(name))
            {

                WorkflowTag tag = new WorkflowTag() { Name = name };
                workflow.Tags.Add(tag);

                await _context.SaveChangesAsync();
            }

            var serializedTags = System.Text.Json.JsonSerializer.Serialize(workflow.Tags);


            return Ok(serializedTags);
        }


        // Post: Workflow/TagAdd/id
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TagAddAjax(int id, string name)
        {
            var workflow = await _context.Workflows
            .Include(w => w.Tags)
            .FirstOrDefaultAsync(m => m.Id == id)
            ;
            if (workflow == null)
            {
                return NotFound();
            }
            if (!workflow.Tags.Any(tag => tag.Name == name))
            {
                WorkflowTag tag = new WorkflowTag() { Name = name };
                workflow.Tags.Add(tag);
                await _context.SaveChangesAsync();
            }

            var serializedTags = System.Text.Json.JsonSerializer.Serialize(workflow.Tags);
            return Json(serializedTags);
        }



        // util methods
        private bool WorkflowExists(int id)
        {
            return _context.Workflows.Any(e => e.Id == id);
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



        //ACTION

        // GET: Workflow/Action/[id]?act=[action]
        public async Task<IActionResult> Action(int? id, string? act)
        {

            String currentUser = getCurrentUser();

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
            actionResponse.Executor = currentUser;




            // engine picks stratagy
            Engine engine = new Engine(_context, workflow);
            workflow = engine.Process(actionResponse);

            TempData["Action_Message"] = "The " + act + " action has been completed.";
            return RedirectToAction(nameof(Details), new { id = id });

        }



    }
}



