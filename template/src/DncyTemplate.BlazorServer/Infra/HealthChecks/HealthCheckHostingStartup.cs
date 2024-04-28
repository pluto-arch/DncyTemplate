using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DncyTemplate.BlazorServer.Infra.HealthChecks
{
    public static class HealthCheckHostingStartup
    {
        /// <inheritdoc />
        public static void ConfigureHealthCheck(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MemoryCheckOptions>(options =>
            {
                options.Threshold = configuration.GetValue<long>("HealthCheck:Memory:Threshold");
            });
            services.AddHealthChecks()
                .AddCheck<MemoryHealthCheck>("memory_check", failureStatus: HealthStatus.Degraded);

            //.AddCheck<DatabaseHealthCheck>("database_check", failureStatus: HealthStatus.Unhealthy,tags: new string[] { "database", "sqlServer" });
        }
    }
}
