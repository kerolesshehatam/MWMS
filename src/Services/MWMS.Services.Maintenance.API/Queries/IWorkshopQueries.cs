
using MWMS.Services.Maintenance.API.Models;
using System;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.API.Queries
{
    public interface IWorkshopQueries
    {
        Task<MaintenanceJobModel> GetMaintenanceJobAsync(DateTime CalendarDate, Guid guid);
        Task<WorkshopCalendarModel> GetWorkshopCalendarAsync(DateTime CalendarDate);


    }
}
