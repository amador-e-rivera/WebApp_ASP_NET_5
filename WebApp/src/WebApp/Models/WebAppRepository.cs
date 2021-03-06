﻿using Microsoft.Data.Entity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApp.Models
{
    public class WebAppRepository : IWebAppRepository
    {
        private WebAppContext _context;
        private ILogger<WebAppRepository> _logger;

        public WebAppRepository(WebAppContext context, ILogger<WebAppRepository> logger)
        {
            _logger = logger;
            _context = context;
        }

        public void AddTrip(Trip newTrip)
        {
            _context.Add(newTrip);
        }

        public void AddStop(Stop newStop, string tripName, string user)
        {
            var trip = getTripByName(tripName, user);
            newStop.Order = trip.Stops.Count > 0 ? trip.Stops.Max(c => c.Order) + 1 : 1;
            trip.Stops.Add(newStop);
            _context.Add(newStop);
        }

        public List<Trip> getAllTrips()
        {
            try
            {
                return _context.Trips.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips from database", ex);
                return null;
            }
        }

        public List<Trip> getAllTripsWithStops()
        {
            try
            {
                return _context.Trips.Include(c => c.Stops).OrderBy(c => c.Name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0; //Greate than zero means saved
        }

        public List<Stop> getAllStops()
        {
            return _context.Stops.ToList();
        }

        public Trip getTripByName(string tripName, string user)
        {
            return _context.Trips.Include(t => t.Stops).Where(t => t.Name == tripName && t.UserName == user).FirstOrDefault();
        }

        public IEnumerable<Trip> getAllUserTripsWithStops(string name)
        {
            try
            {
                return _context.Trips.Include(c => c.Stops).OrderBy(c => c.Name).Where(c => c.UserName == name).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError("Could not get trips with stops from database", ex);
                return null;
            }
        }
    }
}
