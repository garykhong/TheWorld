using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.Services;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("/api/trips/{tripName}/stops")]
    public class StopsController : Controller
    {
        public StopsController(IWorldRepository repository, ILogger<StopsController> logger,
            GeoCoordsService coordsService)
        {
            this.Repository = repository;
            this.Logger = logger;
            this.CoordsService = coordsService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = Repository.GetUserTripByName(tripName, User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>
                    (trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch(Exception ex)
            {
                Logger.LogError("Failed to get stops: {0}", ex);
            }

            return BadRequest("Failed to get stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel vm)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);

                    var result = await CoordsService.GetCoordsAsync(newStop.Name);
                    if(!result.Success)
                    {
                        Logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        Repository.AddStop(tripName, newStop, User.Identity.Name);

                        if (await Repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}",
                            Mapper.Map<StopViewModel>(newStop));
                        }
                    }                                      
                }
            }
            catch(Exception ex)
            {
                Logger.LogError("Failed to save a new Stop: {0}", ex);
            }

            return BadRequest("Failed to save new stop");
        }

        public ILogger<StopsController> Logger { get; private set; }
        public IWorldRepository Repository { get; private set; }
        public GeoCoordsService CoordsService { get; private set; }
    }
}
