﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
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
        private GeoCoordService _coordsService;
        private ILogger<StopsController> _logger;
        private IWorldRepository _repository;

        public StopsController(IWorldRepository repository, 
            ILogger<StopsController> logger,
            GeoCoordService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                var trip = _repository.GetTripByName(tripName, User.Identity.Name);
                
                return Ok(Mapper.Map<IEnumerable<StopViewModel>>
                    (trip.Stops.OrderBy(s => s.Order).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get stops: {ex}");  
            }
            return BadRequest("Failed to get stops");
        }


        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName, [FromBody]StopViewModel stopVM)
        {
            try
            {
                // if VM is valid 
                if (ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(stopVM);
                    // Look up geo codes

                    var result = await _coordsService.GetCoordsAsync(newStop.Name);
                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }
                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;

                        //Save to the Database
                        _repository.AddStop(tripName, User.Identity.Name, newStop);

                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to save new Stop: {0}", ex);
            }
            return BadRequest("Failed to get stops");
        }
    }
}
