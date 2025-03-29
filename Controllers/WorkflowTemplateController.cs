using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            return View(await _context.WorkflowTemplate.ToListAsync());
        }

        // GET: WorkflowTemplate/Details/5
        public async Task<IActionResult> Details(int? id)
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
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
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

        private bool WorkflowTemplateExists(int id)
        {
            return _context.WorkflowTemplate.Any(e => e.Id == id);
        }
    }
}
