using System.Collections.Generic;

namespace WebApp.Models
{
    public interface IWebAppRepository
    {
        List<Trip> getAllTrips();
        List<Trip> getAllTripsWithStops();
        Trip getTripByName(string tripName);
        void AddTrip(Trip newTrip);
        void AddStop(Stop newStop, string tripName);
        bool SaveAll();
        List<Stop> getAllStops();
    }
}