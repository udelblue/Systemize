using Microsoft.EntityFrameworkCore;
using Systemize.Models;

namespace Systemize.Data
{
    public class SystemizeContext : DbContext
    {
        public SystemizeContext(DbContextOptions<SystemizeContext> options) : base(options)
        {
        }

        public DbSet<Document> Documents { get; set; }
        public DbSet<History> History { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Workflow> Workflows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Document>().ToTable("Document");
            modelBuilder.Entity<Link>().ToTable("Link");
            modelBuilder.Entity<History>().ToTable("History");
            modelBuilder.Entity<Stage>().ToTable("Stage");
            modelBuilder.Entity<Workflow>().ToTable("Workflow");

            modelBuilder.Entity<Workflow>()
                .HasMany(w => w.Stages);
            modelBuilder.Entity<Workflow>()
                .HasMany(s => s.History);
            modelBuilder.Entity<Workflow>()
                .HasMany(w => w.Documents);
            modelBuilder.Entity<Workflow>()
                .HasMany(w => w.Links);




        }
    }
}
