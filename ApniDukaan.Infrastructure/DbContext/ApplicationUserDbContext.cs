using ApniDukaan.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace ApniDukaan.Infrastructure.DBContext
{
    public class ApplicationUserDbContext : DbContext
    {
        public DbSet<ApplicationUser> Users { get; set; }  // Represents the Users table in the database

        public ApplicationUserDbContext(DbContextOptions<ApplicationUserDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>().ToTable("ApplicationUsers");

            //// Seed data for the ApplicationUser entity
            //// It adds initial data (initial rows) to the database when the database is newly created.
            //// It will not be reinserted if you delete
            //modelBuilder.Entity<ApplicationUser>().HasData(
            //    new ApplicationUser()
            //    {
            //        UserId = Guid.Parse("9194ab4e-f176-4e28-9bfb-e11d377b4489"),
            //        Email = "seed@example.com",
            //        PasswordHash = "seedpassword",
            //        PersonName = "Seed User",
            //        Gender = "Male"
            //    });                

            //var assembly = typeof(ApplicationUserDbContext).Assembly;
            //using var stream = assembly.GetManifestResourceStream("ApniDukaan.Infrastructure.SeedApplicationUserData.json");
            //using var reader = new StreamReader(stream ?? throw new FileNotFoundException("Embedded resource not found"));
            //var seedData = reader.ReadToEnd();
            //List<ApplicationUser>? applicationUsers = JsonSerializer.Deserialize<List<ApplicationUser>>(seedData);
            //if (applicationUsers != null)
            //{
            //    foreach (var user in applicationUsers)
            //    {
            //        modelBuilder.Entity<ApplicationUser>().HasData(user);
            //    }
            //}

        }
    }
}
