using Microsoft.Extensions.DependencyInjection;

namespace MWMS.Services.Maintenance.API.Queries
{
    public static class QueriesDIRegistration
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddTransient<IWorkshopQueries, WorkshopQueries>();
        }
    }
}
