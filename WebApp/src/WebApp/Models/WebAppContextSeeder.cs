﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class WebAppContextSeeder
    {
        private WebAppContext _context;

        public WebAppContextSeeder(WebAppContext context)
        {
            _context = context;
        }

        public void SeedDatabase()
        {
            if(!_context.Trips.Any())
            {
                Trip euroTrip = new Trip()
                {
                    Name = "Europe Trip 2015",
                    Created = new DateTime(2015, 07, 01),
                    UserName = "Amador Rivera",
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