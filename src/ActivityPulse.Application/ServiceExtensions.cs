using Microsoft.Extensions.DependencyInjection;

namespace ActivityPulse.Application
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ITrackingService, TrackingService>();
            services.AddScoped<IStatisticService, StatisticService>();
            return services;
        }
    }
}
