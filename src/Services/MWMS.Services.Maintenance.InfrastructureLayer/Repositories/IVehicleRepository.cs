using MWMS.Services.Maintenance.InfrastructureLayer.MongoDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.InfrastructureLayer.Repositories
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetVehiclesAsync();
        Task<Vehicle> GetVehicleAsync(string licenseNumber);
    }
}
