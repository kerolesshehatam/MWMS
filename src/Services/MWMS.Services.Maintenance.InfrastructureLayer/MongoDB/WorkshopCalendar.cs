using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MWMS.Services.Maintenance.InfrastructureLayer.MongoDB
{
    public class WorkshopCalendar
    {
        [BsonId]
        public ObjectId InternalId { get; }
        //ID is date
        public string Date { get; set; }
        public int CurrentVersion { get; set; }
    }
}
