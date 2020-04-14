using Microsoft.Extensions.DependencyInjection;
using MWMS.Services.Maintenance.InfrastructureLayer.Repositories;

namespace MWMS.Services.Maintenance.InfrastructureLayer.Util
{
    public static class InfrastructureLayerDIRegistration
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseSettings, DatabaseSettings>();
            services.AddTransient<IVehicleRepository, VehicleRepository>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            services.AddTransient<IWorkshopCalendarRepository, WorkshopCalendarRepository>();


        }
    }
}
