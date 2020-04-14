using MWMS.Messaging.Infrastructure;
using MWMS.Services.Maintenance.InfrastructureLayer.MongoDB;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.InfrastructureLayer.Repositories
{
    public interface IWorkshopCalendarRepository
    {
        Task<WorkshopCalendarEvent> GetMaintenanceJobAsync(DateTime date, Guid guid);
        Task<(WorkshopCalendar, IEnumerable<WorkshopCalendarEvent>)> GetWorkshopCalendarAsync(DateTime date);

        Task<bool> SaveWorkshopCalendarAsync(string calendarId, int originalVersion, int newVersion, IEnumerable<Event> newEvents);
    }
}
