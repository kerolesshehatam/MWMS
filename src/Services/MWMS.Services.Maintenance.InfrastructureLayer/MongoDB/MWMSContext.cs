using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MWMS.Services.Maintenance.InfrastructureLayer.Util;

namespace MWMS.Services.Maintenance.InfrastructureLayer.MongoDB
{
    public class MWMSContext
    {
        private readonly IMongoDatabase _database = null;

        public MWMSContext(IOptions<DatabaseSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            if (client != null)
                _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<Customer> Customers => _database.GetCollection<Customer>("Customer");

        public IMongoCollection<Vehicle> Vehicles => _database.GetCollection<Vehicle>("Vehicle");

        public IMongoCollection<WorkshopCalendar> WorkshopPlans => _database.GetCollection<WorkshopCalendar>("WorkshopCalendar");

        public IMongoCollection<WorkshopCalendarEvent> WorkshopCalendarEvents => _database.GetCollection<WorkshopCalendarEvent>("WorkshopCalendarEvent");
    }
}
