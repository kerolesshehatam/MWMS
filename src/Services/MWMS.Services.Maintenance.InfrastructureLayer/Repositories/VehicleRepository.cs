using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MWMS.Services.Maintenance.InfrastructureLayer.MongoDB;
using MWMS.Services.Maintenance.InfrastructureLayer.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.InfrastructureLayer.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly MWMSContext _context = null;

        public VehicleRepository(IOptions<DatabaseSettings> settings)
        {
            _context = new MWMSContext(settings);
        }
        public async Task<Vehicle> GetVehicleAsync(string licenseNumber)
        {
            return await _context.Vehicles.Find(c => c.LicenseNumber == licenseNumber).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesAsync()
        {
            return await _context.Vehicles.Find(_ => true).ToListAsync();
        }
    }
}
