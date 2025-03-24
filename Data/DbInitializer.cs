using Microsoft.EntityFrameworkCore;
using Systemize.Models;

namespace Systemize.Data;
public static class DbInitializer
{
    public static void Initialize(SystemizeContext context)
    {
        context.Database.Migrate();
        // Look for any students.
        if (!context.Workflows.Any())
        {

            var wf1 = new Workflow
            {
                CurrentStageId = 0,
                Name = "Workflow 1",
                Description = "The first test workflow"
                // EnrollmentDate = DateTime.Parse("2016-09-01")
            };

            wf1.Stages = new List<Stage>
            {
                new Stage { Name = "Stage 1", Description = "The first stage" },
                new Stage { Name = "Stage 2", Description = "The second stage" },
                new Stage { Name = "Stage 3", Description = "The third stage" }
            };



            var wf2 = new Workflow
            {
                CurrentStageId = 0,
                Name = "Workflow 2",
                Description = "The Second test workflow"
                // EnrollmentDate = DateTime.Parse("2016-09-01")
            };

            wf2.Stages = new List<Stage>
            {
                new Stage { Name = "Stage 1", Description = "The first stage" },
                new Stage { Name = "Stage 2", Description = "The second stage" },
                new Stage { Name = "Stage 3", Description = "The third stage" }
            };

            context.AddRange(wf1, wf2);


            context.SaveChanges();

            // DB has been seeded
        }


    }
}
