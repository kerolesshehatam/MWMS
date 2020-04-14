using Microsoft.Extensions.DependencyInjection;

namespace MWMS.Services.Maintenance.API.CommandHandlers
{
    public static class CommandHandlersDIRegistration
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IPlanMaintenanceJobCommandHandler, PlanMaintenanceJobCommandHandler>();
            services.AddTransient<IFinishMaintenanceJobCommandHandler, FinishMaintenanceJobCommandHandler>();
        }
    }
}
