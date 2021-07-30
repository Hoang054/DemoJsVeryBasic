using Microsoft.EntityFrameworkCore;
using ProjectTeamNET.Models.Entity;

namespace ProjectTeamNET.Models
{
    public class ProjectDbContext : DbContext
    {
        public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
        {
        }
        public DbSet<ProcessingMonth> ProcessingMonths { get; set; }
        public DbSet<Manhour> Manhours { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<SalesObject> SaleObjects { get; set; }
        public DbSet<WorkContents> WorkContents { get; set; }
        public DbSet<UserScreenItem> UserScreenItems { get; set; }
        public DbSet<Theme> Themes { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Manhour>()
                   .HasKey(b => b.Year);
            modelBuilder.Entity<WorkContents>()
        .HasKey(c => new { c.Work_contents_code });
            modelBuilder.Entity<Calendar>()
               .HasKey(b => b.Date);

            modelBuilder.Entity<Manhour>()
        .HasKey(sc => new { sc.Year, sc.Month, sc.User_no, sc.Theme_no, sc.Work_contents_class, sc.Work_contents_code, sc.Work_contents_detail });

        }
    }
}
