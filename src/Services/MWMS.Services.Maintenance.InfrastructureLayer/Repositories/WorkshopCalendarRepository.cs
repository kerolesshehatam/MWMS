using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MWMS.Messaging.Infrastructure;
using MWMS.Services.Maintenance.Doamin.Events;
using MWMS.Services.Maintenance.InfrastructureLayer.MongoDB;
using MWMS.Services.Maintenance.InfrastructureLayer.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.InfrastructureLayer.Repositories
{
    public class WorkshopCalendarRepository : IWorkshopCalendarRepository
    {
        private readonly MWMSContext _context = null;

        public WorkshopCalendarRepository(IOptions<DatabaseSettings> settings)
        {
            _context = new MWMSContext(settings);
        }

        public async Task<WorkshopCalendarEvent> GetMaintenanceJobAsync(DateTime date, Guid guid)
        {

            var workshopCalendarEvents = await _context.WorkshopCalendarEvents.Find(w => w.EventDate == date.ToString("yyyy-MM-dd")).FirstOrDefaultAsync();

            return workshopCalendarEvents;
        }

        public async Task<(WorkshopCalendar, IEnumerable<WorkshopCalendarEvent>)> GetWorkshopCalendarAsync(DateTime date)
        {
            var workshopPlan = await _context.WorkshopPlans.Find(w => w.Date == date.ToString("yyyy-MM-dd")).FirstOrDefaultAsync();

            if (workshopPlan == null)
            {
                return (null, null);
            }
            var workshopCalendarEvents = await _context.WorkshopCalendarEvents.Find(w => w.EventDate == date.ToString("yyyy-MM-dd")).ToListAsync();

            return (workshopPlan, workshopCalendarEvents);
        }

        public async Task<bool> SaveWorkshopCalendarAsync(string calendarId, int originalVersion, int newVersion, IEnumerable<Event> newEvents)
        {
            try
            {
                foreach (var newEvent in newEvents.OrderBy(e => e.Priority).ToList())
                {
                    switch (newEvent)
                    {
                        case WorkshopCalendarCreated e:
                            await HandleCommand(newVersion, newEvent as WorkshopCalendarCreated);
                            break;
                        case MaintenanceJobPlanned e:
                            await HandleCommand(calendarId, newEvent as MaintenanceJobPlanned);
                            break;
                        case MaintenanceJobFinished e:
                            await HandleCommand(calendarId, newEvent as MaintenanceJobFinished);
                            break;
                        default:
                            return false;
                    }

                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> HandleCommand(int newVersion, WorkshopCalendarCreated newEvent)
        {
            WorkshopCalendar calendar = new WorkshopCalendar()
            {
                Date = newEvent.Date.ToString("yyyy-MM-dd"),
                CurrentVersion = newVersion
            };
            try
            {
                await _context.WorkshopPlans.InsertOneAsync(calendar);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> HandleCommand(string calendarId, MaintenanceJobPlanned newEvent)
        {
            WorkshopCalendarEvent calendarEvent = new WorkshopCalendarEvent()
            {
                EventDate = calendarId,
                CustomerId = newEvent.CustomerInfo.Id,
                VehicleLicenseNumber = newEvent.VehicleInfo.LicenseNumber,
                Description = newEvent.Description,
                Note = newEvent.Description,
                PlannedEndDateTime = newEvent.StartTime,
                PlannedStartDateTime = newEvent.EndTime,
                MessageType = newEvent.MessageType
            };
            try
            {
                await _context.WorkshopCalendarEvents.InsertOneAsync(calendarEvent);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<bool> HandleCommand(string calendarId, MaintenanceJobFinished newEvent)
        {
            WorkshopCalendarEvent calendarEvent = new WorkshopCalendarEvent()
            {
                EventDate = calendarId,
                PlannedEndDateTime = newEvent.StartTime,
                PlannedStartDateTime = newEvent.EndTime,
                MessageType = newEvent.MessageType
            };
            try
            {
                ReplaceOneResult actionResult = await _context.WorkshopCalendarEvents.ReplaceOneAsync(e => e.Id == newEvent.JobId && e.EventDate == calendarId,
                                                                                                     calendarEvent
                                                                                                     , new ReplaceOptions { IsUpsert = true });
                return actionResult.IsAcknowledged
                    && actionResult.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
