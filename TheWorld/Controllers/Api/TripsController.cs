using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Route("api/trips")]
    public class TripsController : Controller
    {
        public TripsController(IWorldRepository repository,
            ILogger<TripsController> logger)
        {
            this.Repository = repository;
            this.Logger = logger;
        }

        public IWorldRepository Repository { get; private set; }
        public ILogger<TripsController> Logger { get; private set; }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = Repository.GetAllTrips();
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch(Exception ex)
            {
                Logger.LogError($"Failed to get All Trips: {ex}");
                return BadRequest("Error occurred");
            }            
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel trip)
        {
            if(ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(trip);
                Repository.AddTrip(newTrip);

                if(await Repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{trip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }                                                 
            }

            return BadRequest("Failed to save changes to the database");
        }
    }
}
