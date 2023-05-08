using AppointmentInfo.Models;
using Microsoft.EntityFrameworkCore;

namespace AppointmentInfo.Data
{
    public class AppointmentDbContext : DbContext
    {
        public AppointmentDbContext(DbContextOptions<AppointmentDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

       public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentType> AppointmentTypes { get; set; }
        public DbSet<Consultant> Consultants { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            modelBuilder.Entity<AppointmentType>().HasData(
                new AppointmentType { Id = 1, Name = "Consultation", DurationInMinutes = 60 },
                new AppointmentType { Id = 2, Name = "Follow-up", DurationInMinutes = 30 },
                new AppointmentType { Id = 3, Name = "Check-up", DurationInMinutes = 45 }
            );

            modelBuilder.Entity<Consultant>().HasData(
                new Consultant { Id = 1, Name = "John Doe" },
                new Consultant { Id = 2, Name = "Jane Doe" },
                new Consultant { Id = 3, Name = "Bob Smith" }
            );
        }

    }
}
