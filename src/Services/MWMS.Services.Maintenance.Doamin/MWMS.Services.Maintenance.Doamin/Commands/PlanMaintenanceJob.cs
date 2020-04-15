using MWMS.Messaging.Infrastructure;
using System;

namespace MWMS.Services.Maintenance.Doamin.Commands
{
    public class PlanMaintenanceJob : Command
    {
        public readonly Guid JobId;
        public readonly DateTime StartTime;
        public readonly DateTime EndTime;
        public readonly (string Id, string Name, string TelephoneNumber) CustomerInfo;
        public readonly (string LicenseNumber, string Brand, string Type) VehicleInfo;
        public readonly string Description;
        public PlanMaintenanceJob(Guid messageId, Guid jobId, DateTime startTime, DateTime endTime,
           string CustomerInfoId, string CustomerInfoName, string CustomerInfoTelephoneNumber,
           string VehicleInfoLicenseNumber, string VehicleInfoBrand, string VehicleInfoType,
           string description) : base(messageId)
        {
            JobId = jobId;
            StartTime = startTime;
            EndTime = endTime;
            CustomerInfo = (CustomerInfoId, CustomerInfoName, CustomerInfoTelephoneNumber);
            VehicleInfo = (VehicleInfoLicenseNumber, VehicleInfoBrand, VehicleInfoType);
            Description = description;
        }
    }
}
