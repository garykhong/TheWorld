using System.Collections.Generic;

namespace TheWorld.Models
{
    public interface IWorldRepository
    {
        WorldContext Context { get; }

        IEnumerable<Trip> GetAllTrips();
    }
}