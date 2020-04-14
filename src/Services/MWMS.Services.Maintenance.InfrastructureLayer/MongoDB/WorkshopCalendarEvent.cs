using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MWMS.Services.Maintenance.InfrastructureLayer.MongoDB
{
    public class WorkshopCalendarEvent
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public Guid Id { get; set; }
        public string EventDate { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        [BsonDateTimeOptions]
        public DateTime PlannedStartDateTime { get; set; }
        [BsonDateTimeOptions]
        public DateTime PlannedEndDateTime { get; set; }
        [BsonDateTimeOptions]
        public DateTime ActualStartDateTime { get; set; }
        [BsonDateTimeOptions]
        public DateTime ActualEndDateTime { get; set; }
        public string MessageType { get; set; }
        public string VehicleLicenseNumber { get; set; }
        public string CustomerId { get; set; }

    }
}
