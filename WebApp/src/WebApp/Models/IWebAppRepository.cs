using System.Collections.Generic;

namespace WebApp.Models
{
    public interface IWebAppRepository
    {
        List<Trip> getAllTrips();
        List<Trip> getAllTripsWithStops();
        void AddTrip(Trip newTrip);
        void AddStop(Stop newStop);
        bool SaveAll();
    }
}