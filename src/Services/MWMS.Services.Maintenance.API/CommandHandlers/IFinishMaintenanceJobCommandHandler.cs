using MWMS.Services.Maintenance.Doamin.Commands;
using System;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.API.CommandHandlers
{
    public interface IFinishMaintenanceJobCommandHandler
    {
        Task<bool> HandleCommandAsync(DateTime CalendarDate, FinishMaintenanceJob command);
    }
}
