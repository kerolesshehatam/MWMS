using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MWMS.Services.Maintenance.InfrastructureLayer.MongoDB;
using MWMS.Services.Maintenance.InfrastructureLayer.Util;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.InfrastructureLayer.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly MWMSContext _context = null;

        public CustomerRepository(IOptions<DatabaseSettings> settings)
        {
            _context = new MWMSContext(settings);
        }

        public async Task<Customer> GetCustomerAsync(string customerId)
        {
            return await _context.Customers.Find(c => c.CustomerId == customerId).FirstOrDefaultAsync();

        }

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.Find(_ => true).ToListAsync();
        }
    }
}
