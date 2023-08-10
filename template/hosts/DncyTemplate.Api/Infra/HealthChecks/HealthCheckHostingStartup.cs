using Microsoft.Extensions.Diagnostics.HealthChecks;

[assembly: HostingStartup(typeof(DncyTemplate.Api.Infra.HealthChecks.HealthCheckHostingStartup))]
namespace DncyTemplate.Api.Infra.HealthChecks
{
    public class HealthCheckHostingStartup : IHostingStartup
    {
        /// <inheritdoc />
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                services.Configure<MemoryCheckOptions>(options =>
                {
                    options.Threshold = context.Configuration.GetValue<long>("HealthCheck:Memory:Threshold");
                });
                services.AddHealthChecks()
                    .AddCheck<MemoryHealthCheck>("memory_check", failureStatus: HealthStatus.Degraded);

                //.AddCheck<DatabaseHealthCheck>("database_check", failureStatus: HealthStatus.Unhealthy,tags: new string[] { "database", "sqlServer" });
            });
        }
    }
}