using Microsoft.AspNetCore.Mvc;
using MWMS.Services.Maintenance.InfrastructureLayer.Repositories;
using System.Threading.Tasks;

namespace MWMS.Services.Maintenance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private ICustomerRepository _customerRepo;

        public CustomerController(ICustomerRepository customerRepo)
        {
            _customerRepo = customerRepo;
        }

        [HttpGet]
        [Route("customers")]
        public async Task<IActionResult> GetCustomers()
        {
            //Should mapp this to the Model
            return Ok(await _customerRepo.GetCustomersAsync());
        }

        [HttpGet]
        [Route("customers/{customerId}")]
        public async Task<IActionResult> GetCustomerByCustomerId(string customerId)
        {
            var customer = await _customerRepo.GetCustomerAsync(customerId);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }
    }
}