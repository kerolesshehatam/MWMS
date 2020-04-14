using System;

namespace MWMS.Services.Maintenance.InfrastructureLayer.MongoDB
{
    public class WorkshopCalendarEventDTO
    {
        public Guid Id { get; set; }
        public string EventDate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public DateTime PlannedStartDateTime { get; set; }
        public DateTime PlannedEndDateTime { get; set; }
        public DateTime ActualStartDateTime { get; set; }
        public DateTime ActualEndDateTime { get; set; }
        public string MessageType { get; set; }
        public string VehicleLicenseNumber { get; set; }
        public string CustomerId { get; set; }

    }
}
