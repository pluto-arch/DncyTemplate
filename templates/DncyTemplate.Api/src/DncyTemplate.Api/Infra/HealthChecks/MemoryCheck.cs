using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace DncyTemplate.Api.Infra.HealthChecks;

public class MemoryCheckOptions
{
    // Failure threshold (in bytes)
    public long Threshold { get; set; } = 1024L * 1024L * 1024L;
}

public class MemoryHealthCheck : IHealthCheck
{

    private readonly IOptionsMonitor<MemoryCheckOptions> _options;


    public MemoryHealthCheck(IOptionsMonitor<MemoryCheckOptions> options)
    {
        _options = options;
    }



    /// <inheritdoc />
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
    {
        var options = _options.Get(context.Registration.Name);
        var allocated = GC.GetTotalMemory(forceFullCollection: false);
        var data = new Dictionary<string, object>()
        {
            { "AllocatedBytes", allocated },
            { "Gen0Collections", GC.CollectionCount(0) },
            { "Gen1Collections", GC.CollectionCount(1) },
            { "Gen2Collections", GC.CollectionCount(2) },
        };

        var status = (allocated < options.Threshold) ?
            HealthStatus.Healthy : HealthStatus.Unhealthy;

        if (status == HealthStatus.Unhealthy)
        {
            // todo send notification to DevOps
        }

        var rate = Math.Round((double)allocated / (double)options.Threshold, 2, MidpointRounding.ToEven);

        return Task.FromResult(new HealthCheckResult(
            status,
            description: $"阈值({options.Threshold})内存使用率：{rate}%.",
            exception: null,
            data: data));
    }
}