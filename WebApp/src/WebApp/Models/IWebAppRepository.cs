using System.Collections.Generic;

namespace WebApp.Models
{
    public interface IWebAppRepository
    {
        List<Trip> getAllTrips();
        List<Trip> getAllTripsWithStops();
        Trip getTripByName(string tripName, string user);
        void AddTrip(Trip newTrip);
        void AddStop(Stop newStop, string tripName, string user);
        bool SaveAll();
        List<Stop> getAllStops();
        IEnumerable<Trip> getAllUserTripsWithStops(string name);
    }
}