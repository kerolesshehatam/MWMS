using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MWMS.Services.Maintenance.InfrastructureLayer.MongoDB
{
    public class Vehicle
    {
        [BsonId]
        public ObjectId InternalId { get; set; }
        public string LicenseNumber { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public string CustomerId { get; set; }
    }
}
