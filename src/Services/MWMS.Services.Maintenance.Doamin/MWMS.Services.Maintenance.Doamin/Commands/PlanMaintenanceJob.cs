using MWMS.Messaging.Infrastructure;
using System;

namespace MWMS.Services.Maintenance.Doamin.Commands
{
    public class PlanMaintenanceJob : Command
    {
        public readonly Guid JobId;
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public  (string Id, string Name, string TelephoneNumber) CustomerInfo;
        public  (string LicenseNumber, string Brand, string Type) VehicleInfo;
        public readonly string Description;

        public PlanMaintenanceJob(Guid messageId, Guid jobId, DateTime startTime, DateTime endTime,
            (string Id, string Name, string TelephoneNumber) customerInfo,
            (string LicenseNumber, string Brand, string Type) vehicleInfo,
            string description) : base(messageId)
        {
            JobId = jobId;
            StartTime = startTime;
            EndTime = endTime;
            CustomerInfo = customerInfo;
            VehicleInfo = vehicleInfo;
            Description = description;
        }
    }
}
