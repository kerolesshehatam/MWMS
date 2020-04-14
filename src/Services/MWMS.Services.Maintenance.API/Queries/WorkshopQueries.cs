using AutoMapper;
using MWMS.Services.Maintenance.API.Models;
using MWMS.Services.Maintenance.InfrastructureLayer.MongoDB;
using MWMS.Services.Maintenance.InfrastructureLayer.Repositories;
using System;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.API.Queries
{
    public class WorkshopQueries : IWorkshopQueries
    {
        private readonly IMapper _mapper;
        private readonly IWorkshopCalendarRepository _calendarRepo;

        public WorkshopQueries(IMapper mapper, IWorkshopCalendarRepository calendarRepo)
        {
            _calendarRepo = calendarRepo;

            _mapper = mapper;
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkshopCalendar, WorkshopCalendarModel>().ReverseMap();
                cfg.CreateMap<WorkshopCalendarEvent, MaintenanceJobModel>().ReverseMap();
            }).CreateMapper();
        }

        public async Task<WorkshopCalendarModel> GetWorkshopCalendarAsync(DateTime calendarDate)
        {
            var plans = await _calendarRepo.GetWorkshopCalendarAsync(calendarDate);

            return _mapper.Map<WorkshopCalendarModel>(plans);

        }

        public async Task<MaintenanceJobModel> GetMaintenanceJobAsync(DateTime calendarDate, Guid jobId)
        {
            return _mapper.Map<MaintenanceJobModel>(await _calendarRepo.GetMaintenanceJobAsync(calendarDate, jobId));

        }
    }
}
