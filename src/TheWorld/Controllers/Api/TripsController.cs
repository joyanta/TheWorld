using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TheWorld.Models;

namespace TheWorld.Controllers.Api
{
    [Authorize]
    [Route("/api/trips")]
    public class TripsController : Controller
    {
        private ILogger<TripsController> _logger;
        private IWorldRepository _repository;

        public TripsController(IWorldRepository repository, 
            ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {

                //var trips = _repository.GetAllTripsWithStops();
                var trips = _repository.GetUserTripsWithStops(User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(trips));
            }
            catch (Exception ex)
            {
                // TODO Logging;
                _logger.LogError($"Failed to get all Trops: {ex}");
                return BadRequest("Error occured");
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var newTrip = Mapper.Map<Trip>(vm);
                newTrip.UserName = User.Identity.Name;

                _logger.LogInformation("Attempting to save new trip");
                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{vm.Name}", 
                        Mapper.Map<TripViewModel>(newTrip));
                }
            }
            return BadRequest("Failed to save the trip");
        }
    }
}
