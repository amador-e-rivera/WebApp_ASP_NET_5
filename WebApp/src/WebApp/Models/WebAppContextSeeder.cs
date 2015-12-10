using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class WebAppContextSeeder
    {
        private WebAppContext _context;
        private ILogger<WebAppContextSeeder> _logger;
        private UserManager<WebAppUser> _userManager;

        public WebAppContextSeeder(WebAppContext context, UserManager<WebAppUser> manager, ILogger<WebAppContextSeeder> logger)
        {
            _context = context;
            _userManager = manager;
            _logger = logger;
        }

        public async Task SeedDatabaseAsync()
        {
            //Seed the Identity tables
            if(await _userManager.FindByEmailAsync("amador.e.rivera@gmail.com") == null)
            {
                WebAppUser user = new WebAppUser()
                {
                    UserName = "aerivera",
                    Email = "amador.e.rivera@gmail.com"
                };

                string password = Startup.Configuration["AppSettings:TestingPassword"];

                var result = await _userManager.CreateAsync(user, password);

                if(!result.Succeeded)
                {
                    _logger.LogError("Could seed Identity user");
                }
                
            }

            //Seed the Trip and Stop tables
            if(!_context.Trips.Any())
            {
                Trip euroTrip = new Trip()
                {
                    Name = "Europe Trip 2015",
                    Created = new DateTime(2015, 07, 01),
                    UserName = "aerivera",
                    Stops = new List<Stop>()
                    {

                        new Stop()
                        {
                            Name = "Los Angeles",
                            Arrival = new DateTime(2015, 09, 20),
                            Latitude = 34.05,
                            Longitude = -118.2500,
                            Order = 1
                        },
                        new Stop()
                        {
                            Name = "London",
                            Arrival = new DateTime(2015, 09, 21),
                            Latitude = 51.5072,
                            Longitude = -0.1275,
                            Order = 1
                        },
                        new Stop()
                        {
                            Name = "Los Angeles",
                            Arrival = new DateTime(2015, 09, 30),
                            Latitude = 34.05,
                            Longitude = -118.2500,
                            Order = 2
                        }
                    }
                };

                _context.Stops.AddRange(euroTrip.Stops);
                _context.Trips.Add(euroTrip);
                _context.SaveChanges();
            }
        }

    }
}
