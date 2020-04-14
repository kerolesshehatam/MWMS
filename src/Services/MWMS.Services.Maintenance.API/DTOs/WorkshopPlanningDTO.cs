using System;
using System.Collections.Generic;

namespace MWMS.Services.Maintenance.API.DTOs
{
    public class WorkshopCalendarDTO
    {
        public DateTime Date { get; set; }
        public List<MaintenanceJobDTO> Jobs { get; set; }
    }
}
