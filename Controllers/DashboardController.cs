﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            string currentUser = getCurrentUser();

            DashboardEntire dashboardEntire = new DashboardEntire();

            //my assigned
            List<Workflow> myassigned = new List<Workflow>();

            // Fetch workflows assigned to the current user
            myassigned = await _context.Workflows
                .Where(w => w.AssignedTo.Contains(currentUser))
                .ToListAsync();

            dashboardEntire.myAssigned = myassigned;


            //my drafts
            List<Workflow> mydrafts = new List<Workflow>();

            // Fetch workflows assigned to the current user
            mydrafts = await _context.Workflows
                .Where(w => w.CreatedBy.Contains(currentUser) & w.Status == "Draft")
                .ToListAsync();

            dashboardEntire.myDrafts = mydrafts;



            //watchlist
            List<Workflow> mywatchlist = new List<Workflow>();
            var wf3 = await _context.Workflows.FirstOrDefaultAsync(m => m.Id == 1011);
            mywatchlist.Add(wf3);

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
                .Include(s => s.Stages)
                .Include(h => h.History)
                .FirstOrDefaultAsync(m => m.Id == id)
                ;
            if (workflow == null)
            {
                return NotFound();
            }

            MetricsEntire metricsEntire = new MetricsEntire();
            List<MetricsRow> metrics = new List<MetricsRow>();

            if (workflow.Stages != null && !workflow.Stages.IsNullOrEmpty())
            {
                foreach (var stage in workflow.Stages)
                {

                    var startHistory = workflow.History.FirstOrDefault(h => h.StageId == stage.Id && h.EventName == "Stage Started");
                    var finishHistory = workflow.History.FirstOrDefault(h => h.StageId == stage.Id && h.EventName == "Stage Completed");
                    int stageval = 0;
                    if (startHistory != null && finishHistory != null)
                    {
                        stageval = (int)(finishHistory.ExecutedAt - startHistory.ExecutedAt).TotalHours;
                    }

                    MetricsRow rw1 = new MetricsRow() { StageName = stage.Name, StageValue = stageval };
                    metrics.Add(rw1);
                }
            }

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
