using MWMS.Messaging.Infrastructure;
using MWMS.Services.Maintenance.Doamin.BusinessRules;
using MWMS.Services.Maintenance.Doamin.Commands;
using MWMS.Services.Maintenance.Doamin.Entities;
using MWMS.Services.Maintenance.Doamin.Events;
using MWMS.Services.Maintenance.Doamin.Exceptions;
using MWMS.Services.Maintenance.Doamin.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MWMS.Services.Maintenance.Doamin.Aggregates
{
    public class WorkshopCalendarAggregateRoot : AggregateRoot<WorkshopCalendarId>
    {
        /// <summary>
        /// The list of maintenance-jobs for this day. 
        /// </summary>
        public List<MaintenanceJob> Jobs { get; private set; }

        public WorkshopCalendarAggregateRoot(DateTime date)
               : base(WorkshopCalendarId.Create(date))
        { }

        public WorkshopCalendarAggregateRoot(DateTime date, IEnumerable<Event> events) :
            base(WorkshopCalendarId.Create(date), events)
        {
            Jobs = new List<MaintenanceJob>();
        }

        /// <summary>
        /// Creates a new instance of a workshop-Calendar for the specified date.
        /// </summary>
        /// <param name="date">The date to create the Calendar for.</param>
        public static WorkshopCalendarAggregateRoot Create(DateTime date)
        {
            //CreateMaintenancePlan command = new CreateMaintenancePlan(Guid.NewGuid(), date);
            WorkshopCalendarAggregateRoot calendar = new WorkshopCalendarAggregateRoot(date);
            WorkshopCalendarCreated createEvent = new WorkshopCalendarCreated(Guid.NewGuid(), date);
            calendar.RaiseEvent(createEvent);
            return calendar;
        }

        public void PlanMaintenanceJob(PlanMaintenanceJob command)
        {
            // check business rules
            command.PlannedMaintenanceJobShouldFallWithinOneBusinessDay();
            this.NumberOfParallelMaintenanceJobsMustNotExceedAvailableWorkStations(command);
            this.NumberOfParallelMaintenanceJobsOnAVehicleMustNotExceedOne(command);

            // handle event
            MaintenanceJobPlanned e = new MaintenanceJobPlanned(command.MessageId,
                                                                command.StartTime,
                                                                command.EndTime,
                                                                command.CustomerInfo,
                                                                command.VehicleInfo,
                                                                command.Description);
            RaiseEvent(e);
        }

        public void FinishMaintenanceJob(FinishMaintenanceJob command)
        {
            // find job
            MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == command.JobId);
            if (job == null)
            {
                throw new MaintenanceJobNotFoundException($"Maintenance job with id {command.JobId} found.");
            }

            // check business rules
            job.FinishedMaintenanceJobCanNotBeFinished();

            // handle event
            MaintenanceJobFinished e = new MaintenanceJobFinished(command.MessageId,
                                                                  command.JobId,
                                                                  command.StartTime,
                                                                  command.EndTime,
                                                                  command.Notes);
            RaiseEvent(e);
        }

        /// <summary>
        /// Handles an event and updates the aggregate version.
        /// </summary>
        /// <remarks>Caution: this handles is also called while replaying events to restore state.
        /// So, do not execute any checks that could fail or introduce any side-effects in this handler.</remarks>
        protected override void When(dynamic @event)
        {
            Handle(@event);
        }

        private void Handle(WorkshopCalendarCreated e)
        {
            Jobs = new List<MaintenanceJob>();
        }

        private void Handle(MaintenanceJobPlanned e)
        {
            MaintenanceJob job = new MaintenanceJob(e.JobId);
            Customer customer = new Customer(e.CustomerInfo.Id, e.CustomerInfo.Name, e.CustomerInfo.TelephoneNumber);
            LicenseNumber licenseNumber = LicenseNumber.Create(e.VehicleInfo.LicenseNumber);
            Vehicle vehicle = new Vehicle(licenseNumber, e.VehicleInfo.Brand, e.VehicleInfo.Type, customer.Id);
            Timeslot plannedTimeslot = Timeslot.Create(e.StartTime, e.EndTime);
            job.Plan(plannedTimeslot, vehicle, customer, e.Description);
            Jobs.Add(job);
        }

        private void Handle(MaintenanceJobFinished e)
        {
            MaintenanceJob job = Jobs.FirstOrDefault(j => j.Id == e.JobId);
            Timeslot actualTimeslot = Timeslot.Create(e.StartTime, e.EndTime);
            job.Finish(actualTimeslot, e.Notes);
        }

    }
}
