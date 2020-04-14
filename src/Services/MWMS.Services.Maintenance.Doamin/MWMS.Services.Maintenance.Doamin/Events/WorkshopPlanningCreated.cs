using MWMS.Messaging.Infrastructure;
using System;

namespace MWMS.Services.Maintenance.Doamin.Events
{
    public class WorkshopCalendarCreated : Event
    {
        public readonly DateTime Date;

        public WorkshopCalendarCreated(Guid messageId, DateTime date) : base(messageId)
        {
            Date = date;
            this.Priority = 1;
        }
    }
}
