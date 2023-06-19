using DncyTemplate.Application.IntegrationEvents.IntegrationEventbox;

namespace DncyTemplate.Mvc.BackgroundServices;

public class PrductBackgroundService : BackgroundService
{

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PrductBackgroundService> _logger;

    public PrductBackgroundService(IServiceScopeFactory scopeFactory, ILogger<PrductBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }


    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(30, stoppingToken);
            await PublishOutstandingIntegrationEvents(stoppingToken);
        }
    }

    private async Task PublishOutstandingIntegrationEvents(CancellationToken stoppingToken)
    {
        try
        {
            await Task.Yield();
            using var scope = _scopeFactory.CreateScope();
            var box = scope.ServiceProvider.GetService<IntegrationEventBoxService>();
            while (box.MemoryBox.TryTake(out string ev))
            {
                // TODO 发送到事件总线 rabbitmq或者其他消息队列 并更新包裹邮递状态
                // TODO set ev publishing
                // TODO eventBus.Publlish();
                // TODO set ev published
                _logger.LogInformation($"[信箱] 发布：{ev}");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "产品发件箱失败：{mesg}", ex.Message);
            throw;
        }
    }
}