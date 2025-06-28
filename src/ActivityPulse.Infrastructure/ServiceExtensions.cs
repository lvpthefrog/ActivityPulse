using ActivityPulse.Application;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace ActivityPulse.Infrastructure
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IDbConnection>(provider => new SqliteConnection($"Data Source={InfraHelper.GetDatabasePath()}"));

            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IUserStateRepository, UserStateRepository>();
            services.AddScoped<ISessionAggregateRepository, SessionAggregateRepository>();
            services.AddScoped<IAppMetaDataRepository, AppMetaDataRepository>();
            services.AddScoped<IUserStateAggregateRepository, UserStateAggregateRepository>();

            services.AddScoped<ISystemProvider, SystemProvider>();
            return services;
        }
    }
}
