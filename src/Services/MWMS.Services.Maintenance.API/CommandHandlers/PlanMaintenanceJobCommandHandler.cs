﻿using AutoMapper;
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
            }).CreateMapper();
        }

        public async Task<bool> HandleCommandAsync(DateTime calendarDate, PlanMaintenanceJob command)
        {
            bool isJobPlannedSuccessfully = false;
            // get or create workshop-Calendar
            var (existingCalendar, existingEvents) = await _calendarRepo.GetWorkshopCalendarAsync(calendarDate);
            WorkshopCalendarAggregateRoot workshopCalendar;

            if (existingCalendar == null)
            {
                workshopCalendar = WorkshopCalendarAggregateRoot.Create(calendarDate);
            }
            else
            {
                workshopCalendar = new WorkshopCalendarAggregateRoot(calendarDate);
            }

            // handle command
            workshopCalendar.PlanMaintenanceJob(command);

            // persist
            IEnumerable<Event> events = workshopCalendar.GetEvents();

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
