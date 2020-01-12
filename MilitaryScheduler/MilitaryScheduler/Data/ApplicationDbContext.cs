using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilitaryScheduler.Models;

namespace MilitaryScheduler.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<CalendarEvent> Events { get; set; }
        public DbSet<RequestModel> Requests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CalendarEvent>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();
            });

            base.OnModelCreating(builder);
        }
    }
}
