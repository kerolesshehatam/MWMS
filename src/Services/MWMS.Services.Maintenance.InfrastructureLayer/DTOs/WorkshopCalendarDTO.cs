using System.Collections.Generic;

namespace MWMS.Services.Maintenance.InfrastructureLayer.MongoDB
{
    public class WorkshopCalendarDTO
    {
        public string Date { get; set; }
        public int CurrentVersion { get; set; }
        public IList<WorkshopCalendarEventDTO> Events { get; set; }
    }
}
