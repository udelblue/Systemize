﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using Systemize.Models;

namespace Systemize.Data
{
    public class SystemizeContext : IdentityDbContext<IdentityUser>
    {
        public SystemizeContext(DbContextOptions<SystemizeContext> options) : base(options)
        {
        }


        public DbSet<WorkflowNote> Notes { get; set; }

        public DbSet<Document> Documents { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Workflow> Workflows { get; set; }
        public DbSet<DocumentTag> DocumentTag { get; set; } = default!;
        public DbSet<WorkflowTag> WorkflowTag { get; set; } = default!;
        public DbSet<WorkflowTemplate> WorkflowTemplate { get; set; } = default!;
        public DbSet<Link> Links { get; set; } = default!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // Ensure Identity configurations are applied


            modelBuilder.Entity<Document>().ToTable("Document");
            modelBuilder.Entity<DocumentTag>().ToTable("Document_Tags");
            modelBuilder.Entity<Link>().ToTable("Link");
            modelBuilder.Entity<History>().ToTable("History");
            modelBuilder.Entity<Stage>().ToTable("Stage");



            modelBuilder.Entity<Workflow>().ToTable("Workflow");
            modelBuilder.Entity<WorkflowTag>().ToTable("Workflow_Tags");
            modelBuilder.Entity<WorkflowTemplate>().ToTable("Workflow_Template");
            modelBuilder.Entity<WorkflowNote>().ToTable("Workflow_Notes");

            //modelBuilder.Entity<WorkflowSetting>().ToTable("WF_Setting");




            //form info as json
            modelBuilder.Entity<Workflow>()
                .OwnsOne(f => f.WorkflowForm)
                .ToJson();



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
            modelBuilder.Entity<Document>()
               .HasMany(d => d.Tags);

            modelBuilder.Entity<Workflow>()
               .HasMany(d => d.Notes);

            modelBuilder.Entity<WorkflowTemplate>()
                .HasMany(w => w.Stages);

            //modelBuilder.Entity<Workflow>().HasOne(w => w.WorkflowSetting);


        }


    }
}
