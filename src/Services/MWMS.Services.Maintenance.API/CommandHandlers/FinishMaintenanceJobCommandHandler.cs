using AutoMapper;
using MWMS.Messaging.Infrastructure;
using MWMS.Services.Maintenance.API.Models;
using MWMS.Services.Maintenance.Doamin.Aggregates;
using MWMS.Services.Maintenance.Doamin.Commands;
using MWMS.Services.Maintenance.Doamin.Entities;
using MWMS.Services.Maintenance.InfrastructureLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.API.CommandHandlers
{
    public class FinishMaintenanceJobCommandHandler : IFinishMaintenanceJobCommandHandler
    {
        private readonly IMapper _mapper;
        private IMessagePublisher _messagePublisher;
        private IWorkshopCalendarRepository _calendarRepo;
        public FinishMaintenanceJobCommandHandler(IMapper mapper, IMessagePublisher messagePublisher, IWorkshopCalendarRepository calendarRepo)
        {
            _mapper = mapper;
            _messagePublisher = messagePublisher;
            _calendarRepo = calendarRepo;

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkshopCalendarAggregateRoot, WorkshopCalendarModel>().ReverseMap();
                cfg.CreateMap<MaintenanceJob, MaintenanceJobModel>().ReverseMap();
            }).CreateMapper();
        }

        public async Task<bool> HandleCommandAsync(DateTime calendarDate, FinishMaintenanceJob command)
        {
            bool isJobFinishedSuccessfully = false;
            // get or create workshop-Calendar
            var (existingCalendar, existingEvents) = await _calendarRepo.GetWorkshopCalendarAsync(calendarDate);

            WorkshopCalendarAggregateRoot workshopCalendar;

            if (existingCalendar == null)
            {
                workshopCalendar = WorkshopCalendarAggregateRoot.Create(calendarDate);
            }
            else
            {
                //TODO: Map existingEvents to the root event so we can check the overlapping
                workshopCalendar = new WorkshopCalendarAggregateRoot(calendarDate);
            }

            // handle command
            workshopCalendar.FinishMaintenanceJob(command);

            // persist
            //TODO we shouldn't use events while save to database events should used only between context 
            //we should use Aggregate root instance values while validated by business rules 
            IEnumerable<Event> events = workshopCalendar.GetEvents();

            isJobFinishedSuccessfully = await _calendarRepo.SaveWorkshopCalendarAsync(workshopCalendar.Id, workshopCalendar.OriginalVersion, workshopCalendar.Version, events);

            // publish event
            foreach (var e in events)
            {
                await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
            }

            // return result
            return isJobFinishedSuccessfully;
        }
    }
}
