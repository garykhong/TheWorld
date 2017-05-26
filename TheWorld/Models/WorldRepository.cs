using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheWorld.Models
{
    public class WorldRepository : IWorldRepository
    {
        public WorldRepository(WorldContext context, ILogger<WorldRepository> logger)
        {
            this.Context = context;
            this.Logger = logger;
        }

        public IEnumerable<Trip> GetAllTrips()
        {
            Logger.LogInformation("Getting All Trips from the Database");
            return Context.Trips.ToList();
        }

        public void AddTrip(Trip newTrip)
        {
            Context.Add(newTrip);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await Context.SaveChangesAsync()) > 0;
        }

        public Trip GetTripByName(string tripName)
        {
            return Context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName)
                .FirstOrDefault();
        }

        public void AddStop(string tripName, Stop newStop, string username)
        {
            var trip = GetUserTripByName(tripName, username);

            if(trip != null)
            {
                trip.Stops.Add(newStop);
                Context.Stops.Add(newStop);
            }
        }

        public IEnumerable<Trip> GetTripsByUsername(string name)
        {
            return Context
                .Trips
                .Where(t => t.UserName == name)
                .ToList();
        }

        public Trip GetUserTripByName(string tripName, string name)
        {
            return Context.Trips
                .Include(t => t.Stops)
                .Where(t => t.Name == tripName && t.UserName == name)
                .FirstOrDefault();
        }

        public WorldContext Context { get; private set; }
        public ILogger<WorldRepository> Logger { get; private set; }
    }
}
