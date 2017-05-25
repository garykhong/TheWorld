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

        public WorldContext Context { get; private set; }
        public ILogger<WorldRepository> Logger { get; private set; }
    }
}
