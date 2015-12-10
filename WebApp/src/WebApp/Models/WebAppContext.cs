using System;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;

namespace WebApp.Models
{

    public class WebAppContext : IdentityDbContext<WebAppUser>
    {
        public DbSet<Trip> Trips { set; get; }
        public DbSet<Stop> Stops { get; set; }

        public WebAppContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = Startup.Configuration["Data:WebAppContextConnection"];

            optionsBuilder.UseSqlServer(connectionString);

            base.OnConfiguring(optionsBuilder);
        }
    }
}
