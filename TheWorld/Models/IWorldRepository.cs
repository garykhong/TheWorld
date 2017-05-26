using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        WorldContext Context { get; }

        IEnumerable<Trip> GetAllTrips();
        void AddTrip(Trip newTrip);
        Task<bool> SaveChangesAsync();
        Trip GetTripByName(string tripName);
        void AddStop(string tripName, Stop newStop, string username);
        IEnumerable<Trip> GetTripsByUsername(string name);
        Trip GetUserTripByName(string tripName, string name);
    }
}