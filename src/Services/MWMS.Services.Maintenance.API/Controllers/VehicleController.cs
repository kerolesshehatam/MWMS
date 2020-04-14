using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MWMS.Services.Maintenance.InfrastructureLayer.Repositories;

namespace MWMS.Services.Maintenance.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
            IVehicleRepository _vehicleRepo;

            public VehicleController(IVehicleRepository vehicleRepo)
            {
                _vehicleRepo = vehicleRepo;
            }

            [HttpGet]
            [Route("vehicles")]
            public async Task<IActionResult> GetVehicles()
            {
                return Ok(await _vehicleRepo.GetVehiclesAsync());
            }

            [HttpGet]
            [Route("vehicles/{licenseNumber}")]
            public async Task<IActionResult> GetVehicleByLicenseNumber(string licenseNumber)
            {
                var vehicle = await _vehicleRepo.GetVehicleAsync(licenseNumber);
                if (vehicle == null)
                {
                    return NotFound();
                }
                return Ok(vehicle);
            }
        
    }
}