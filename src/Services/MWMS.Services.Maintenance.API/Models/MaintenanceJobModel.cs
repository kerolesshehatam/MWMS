using System;

namespace MWMS.Services.Maintenance.API.Models
{
    public class MaintenanceJobModel
    {
        public Guid Id { get; set; }
        //ID of workshop calendar
        public string EventDate { get; set; }
        public string VehicleLicenseNumber { get; set; }
        public string CustomerId { get; set; }
        public DateTime? PlannedStartDateTime { get; set; }
        public DateTime? PlannedEndDateTime { get; set; }
        public DateTime? ActualStartDateTime { get; set; }
        public DateTime? ActualEndDateTime { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string Status => (!ActualStartDateTime.HasValue && !ActualEndDateTime.HasValue) ? "Planned" : "Completed";

    }
}
