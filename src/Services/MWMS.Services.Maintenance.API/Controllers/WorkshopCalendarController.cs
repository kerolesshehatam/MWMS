using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MWMS.Services.Maintenance.API.CommandHandlers;
using MWMS.Services.Maintenance.API.Models;
using MWMS.Services.Maintenance.API.Queries;
using MWMS.Services.Maintenance.Doamin.Commands;
using MWMS.Services.Maintenance.Doamin.Exceptions;
using MWMS.Services.Maintenance.InfrastructureLayer.Exceptions;
using Serilog;
using System;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkshopCalendarController : ControllerBase
    {
        private readonly IWorkshopQueries _workshopQueries;
        private readonly IPlanMaintenanceJobCommandHandler _planMaintenanceJobCommandHandler;
        private readonly IFinishMaintenanceJobCommandHandler _finishMaintenanceJobCommandHandler;

        public WorkshopCalendarController(
                IWorkshopQueries workshopQueries,
                IPlanMaintenanceJobCommandHandler planMaintenanceJobCommandHandler,
                IFinishMaintenanceJobCommandHandler finishMaintenanceJobCommand)
        {
            _workshopQueries = workshopQueries;
            _planMaintenanceJobCommandHandler = planMaintenanceJobCommandHandler;
            _finishMaintenanceJobCommandHandler = finishMaintenanceJobCommand;
        }

        [HttpGet]
        [Route("{calendarDate}", Name = "GetByDate")]
        public async Task<IActionResult> GetByDate(DateTime calendarDate)
        {
            try
            {
                var calendar = await _workshopQueries.GetWorkshopCalendarAsync(calendarDate);
                if (calendar == null)
                {
                    return NotFound();
                }

                return Ok(calendar);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
        }

        [HttpGet]
        [Route("{calendarDate}/jobs/{jobId}")]
        public async Task<IActionResult> GetMaintenanceJobAsync(DateTime calendarDate, Guid jobId)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var job = await _workshopQueries.GetMaintenanceJobAsync(calendarDate, jobId);
                    if (job == null)
                    {
                        return NotFound();
                    }
                    return Ok(job);
                }
                catch (Exception ex)
                {
                    Log.Error(ex.ToString());
                    throw;
                }
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("{calendarDate}/jobs")]
        public async Task<IActionResult> PlanMaintenanceJobAsync(DateTime calendarDate, [FromBody] PlanMaintenanceJob command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // handle command
                        var plannedSuccessfully = await _planMaintenanceJobCommandHandler.HandleCommandAsync(calendarDate, command);

                        // handle result    
                        if (!plannedSuccessfully)
                        {
                            return NotFound();
                        }

                        // return result
                        return Ok(plannedSuccessfully);
                    }
                    catch (BusinessRuleViolationException ex)
                    {
                        return StatusCode(StatusCodes.Status409Conflict, new ViolationModel { ErrorMessage = ex.Message });
                    }
                }
                return BadRequest();
            }
            catch (ConcurrencyException)
            {
                string errorMessage = "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.";
                Log.Error(errorMessage);
                ModelState.AddModelError("ErrorMessage", errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Route("{CalendarDate}/jobs/{jobId}/finish")]
        public async Task<IActionResult> FinishMaintenanceJobAsync(DateTime calendarDate, Guid jobId, [FromBody] FinishMaintenanceJob command)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // handle command
                    var finished = await _finishMaintenanceJobCommandHandler.HandleCommandAsync(calendarDate, command);

                    // handle result    
                    if (!finished)
                    {
                        return NotFound();
                    }

                    // return result
                    return Ok(finished);
                }
                return BadRequest();
            }
            catch (ConcurrencyException)
            {
                string errorMessage = "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.";
                Log.Error(errorMessage);
                ModelState.AddModelError("", errorMessage);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }


    }
}