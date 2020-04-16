using MWMS.Messaging.Infrastructure;
using System;

namespace MWMS.Services.Maintenance.Doamin.Commands
{
    public class CreateMaintenancePlan : Command
    {
        public readonly DateTime _calendarDate;

        public CreateMaintenancePlan(Guid messageId, DateTime calendarDate) : base(messageId)
        {
            _calendarDate = calendarDate;
        }
    }
}
