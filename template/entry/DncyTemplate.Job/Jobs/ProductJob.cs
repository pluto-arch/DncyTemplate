using DncyTemplate.Domain.Aggregates.Product;
using DncyTemplate.Domain.Repository;
using DncyTemplate.Job.Infra;

using Microsoft.EntityFrameworkCore;

using Quartz;

namespace DncyTemplate.Job.Jobs;

[DisallowConcurrentExecution]
public class ProductJob : IJob, IBackgroundJob
{
    private readonly ILogger<ProductJob> _logger;
    private readonly IRepository<Product> _productsRepo;

    public ProductJob(
        ILogger<ProductJob> logger,
        IRepository<Product> productsRepo)
    {
        _logger = logger;
        _productsRepo = productsRepo;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        int count = await _productsRepo.IgnoreQueryFilters().CountAsync();
        await Task.Delay(1000);
        _logger.LogInformation("设备总数：{count}", count);
        // TODO operator database
        context.Result = JsonConvert.SerializeObject(new { deviceCount = count });
    }
}