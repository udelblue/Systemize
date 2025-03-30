using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Systemize.Models;

namespace Systemize.Data
{
    public class SystemizeContext : IdentityDbContext<IdentityUser>
    {
        public SystemizeContext(DbContextOptions<SystemizeContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Workflow> Workflows { get; set; }

        public DbSet<Systemize.Models.WorkflowTag> WorkflowTag { get; set; } = default!;
        public DbSet<Systemize.Models.WorkflowTemplate> WorkflowTemplate { get; set; } = default!;

        public DbSet<Link> Links { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity configurations are applied


            modelBuilder.Entity<Document>().ToTable("Document");
            modelBuilder.Entity<Link>().ToTable("Link");
            modelBuilder.Entity<History>().ToTable("History");
            modelBuilder.Entity<Stage>().ToTable("Stage");
            modelBuilder.Entity<Workflow>().ToTable("Workflow");
            modelBuilder.Entity<WorkflowTag>().ToTable("Workflow_Tags");
            modelBuilder.Entity<WorkflowTemplate>().ToTable("Workflow_Template");


            //modelBuilder.Entity<WorkflowSetting>().ToTable("WF_Setting");

            modelBuilder.Entity<Workflow>()
                .HasMany(w => w.Stages);
            modelBuilder.Entity<Workflow>()
                .HasMany(s => s.History);
            modelBuilder.Entity<Workflow>()
                .HasMany(s => s.Tags);
            modelBuilder.Entity<Workflow>()
                .HasMany(w => w.Documents);
            modelBuilder.Entity<Workflow>()
                .HasMany(w => w.Links);


            modelBuilder.Entity<WorkflowTemplate>()
                .HasMany(w => w.Stages);

            //modelBuilder.Entity<Workflow>().HasOne(w => w.WorkflowSetting);


        }


    }
}
