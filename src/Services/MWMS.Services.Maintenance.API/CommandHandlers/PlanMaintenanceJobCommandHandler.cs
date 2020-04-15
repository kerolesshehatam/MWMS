using AutoMapper;
using MWMS.Messaging.Infrastructure;
using MWMS.Services.Maintenance.API.Models;
using MWMS.Services.Maintenance.Doamin.Aggregates;
using MWMS.Services.Maintenance.Doamin.Commands;
using MWMS.Services.Maintenance.Doamin.Entities;
using MWMS.Services.Maintenance.InfrastructureLayer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.API.CommandHandlers
{
    public class PlanMaintenanceJobCommandHandler : IPlanMaintenanceJobCommandHandler
    {
        private readonly IMapper _mapper;
        private IMessagePublisher _messagePublisher;
        private IWorkshopCalendarRepository _calendarRepo;

        public PlanMaintenanceJobCommandHandler(IMapper mapper, IMessagePublisher messagePublisher, IWorkshopCalendarRepository calendarRepo)
        {
            _messagePublisher = messagePublisher;
            _calendarRepo = calendarRepo;
            _mapper = mapper;

            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<WorkshopCalendarAggregateRoot, WorkshopCalendarModel>().ReverseMap();
                cfg.CreateMap<MaintenanceJob, MaintenanceJobModel>().ReverseMap();
                cfg.CreateMap<MaintenanceJob, MaintenanceJobModel>().ReverseMap();
            }).CreateMapper();
        }

        public async Task<bool> HandleCommandAsync(DateTime calendarDate, PlanMaintenanceJob command)
        {
            bool isJobPlannedSuccessfully = false;
            // get or create workshop-Calendar
            //(WorkshopCalendar existingCalendar, IEnumerable<WorkshopCalendarEvent> existingEvents)
            var (existingCalendar, existingEvents) = await _calendarRepo.GetWorkshopCalendarAsync(calendarDate);
            WorkshopCalendarAggregateRoot workshopCalendar;

            if (existingCalendar == null)
            {
                workshopCalendar = WorkshopCalendarAggregateRoot.Create(calendarDate);
            }
            else
            {
                //TODO: Map existingEvents to the root event so we can check the overlapping
                //var activeEvents = existingEvents?.Where(e => e.ActualEndDateTime != null)?.ToList();
                //if (activeEvents.Any())
                //{
                //    events.Concat(activeEvents);
                //}
                workshopCalendar = new WorkshopCalendarAggregateRoot(calendarDate, new List<Event>());
            }

            // handle command
            //TODO while planning for a new maintenance job, system should create new GUID for the new JOB
            workshopCalendar.PlanMaintenanceJob(command);

            IEnumerable<Event> events = workshopCalendar.GetEvents();

            // persist
            //TODO we shouldn't use events while save to database events should used only between context 
            //we should use Aggregate root instance values while validated by business rules 
            isJobPlannedSuccessfully = await _calendarRepo.SaveWorkshopCalendarAsync(workshopCalendar.Id, workshopCalendar.OriginalVersion, workshopCalendar.Version, events);

            // publish event
            foreach (var e in events)
            {
                await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
            }

            // return result
            return isJobPlannedSuccessfully;
        }
    }
}
