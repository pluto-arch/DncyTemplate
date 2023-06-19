using DncyTemplate.Application.IntegrationEvents.IntegrationEventbox;

namespace DncyTemplate.Api.BackgroundServices;

public class PrductBackgroundService : BackgroundService
{

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<PrductBackgroundService> _logger;

    public PrductBackgroundService(IServiceScopeFactory scopeFactory, ILogger<PrductBackgroundService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }


    private readonly TimeSpan _period = TimeSpan.FromSeconds(1);

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new PeriodicTimer(_period);

        while (!stoppingToken.IsCancellationRequested
               && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await PublishOutstandingIntegrationEvents(stoppingToken);
        }
    }

    private async Task PublishOutstandingIntegrationEvents(CancellationToken stoppingToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var box = scope.ServiceProvider.GetService<IntegrationEventBoxService>();
            while (box.MemoryBox.TryTake(out string ev))
            {
                _logger.LogInformation("[信箱] 发布：{ev}",ev);
                // TODO 发送到事件总线 rabbitmq或者其他消息队列 并更新包裹邮递状态
                await box.SetEventPublishing();
                // TODO eventBus.Publlish();
                await box.SetEventPublished();

            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "产品发件箱失败：{mesg}", ex.Message);
        }
    }
}