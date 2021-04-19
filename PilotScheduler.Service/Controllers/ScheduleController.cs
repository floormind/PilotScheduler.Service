using System;
using Microsoft.AspNetCore.Mvc;
using PilotScheduler.Repository.Interfaces;
using PilotScheduler.Service.Models.Classes;

namespace PilotScheduler.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScheduleController : ControllerBase
    {
        private readonly IDummyDatabaseRepository _dummyDatabaseRepository;
        public ScheduleController(IDummyDatabaseRepository dummyDatabaseRepository)
        {
            _dummyDatabaseRepository = dummyDatabaseRepository;
        }

        [HttpGet("request-pilot/{location}/{depDateTime}/{returnDateTime}")]
        public IActionResult RequestPilot(string location, string depDateTime, string returnDateTime)
        {
            try
            {
                if (String.IsNullOrEmpty(location))
                    return BadRequest("Invalid location provided");
                
                DateTime parsedDepartureDateTime;
                DateTime parsedReturnDateTime;
                
                if(!DateTime.TryParse(depDateTime, out parsedDepartureDateTime) 
                   || !DateTime.TryParse(returnDateTime, out parsedReturnDateTime))
                    return BadRequest("Invalid date provided");

                var pilot = _dummyDatabaseRepository.RequestPilot(location, parsedDepartureDateTime, parsedReturnDateTime);
                
                return Ok(pilot);
            }
            catch (Exception e)
            {
                return Problem("There seems to be an error, we are looking into this for you.");
            }
        }

        [HttpPost]
        public IActionResult ScheduleFlight([FromBody] ScheduleRequestModel scheduleRequestModel)
        {
            try
            {
                var scheduledPilot = _dummyDatabaseRepository.ScheduleFlight(scheduleRequestModel.PilotId,
                    scheduleRequestModel.DepDateTime, scheduleRequestModel.ReturnDateTime);

                if (null == scheduledPilot)
                    return NotFound("Pilot not found");

                return Ok(scheduledPilot);
            }
            catch (Exception e)
            {
                return Problem("There seems to be an error, we are looking into this for you.");
            }
        }
    }
}
