using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        IEnumerable<Trip> GetAllTrips();
       
        Trip GetTripByName(string tripName, string userName);

        IEnumerable<Trip> GetAllTripsWithStops();

        IEnumerable<Trip> GetUserTripsWithStops(string name);
        
        void AddTrip(Trip newTrip);

        void AddStop(string tripName, string userName, Stop newStop);

        Task<bool> SaveChangesAsync();
    }
}