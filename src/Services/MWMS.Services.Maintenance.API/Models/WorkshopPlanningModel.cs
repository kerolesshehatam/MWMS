using System;
using System.Collections.Generic;

namespace MWMS.Services.Maintenance.API.Models
{
    public class WorkshopCalendarModel
    {
        public string Date { get; set; }
        public IEnumerable<MaintenanceJobModel> Jobs { get; set; }
    }
}
