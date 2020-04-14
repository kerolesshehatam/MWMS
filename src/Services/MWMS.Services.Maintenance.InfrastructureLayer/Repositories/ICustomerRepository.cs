using MWMS.Services.Maintenance.InfrastructureLayer.MongoDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.InfrastructureLayer.Repositories
{
    public interface ICustomerRepository
    {
       Task<IEnumerable<Customer>> GetCustomersAsync();
        Task<Customer> GetCustomerAsync(string customerId);

    }
}
