using DncyTemplate.Domain.DomainEvents.Product;


namespace DncyTemplate.Application.DomainEventHandlers.Product;

[AutoResolveDependency]
public partial class NewProductCreateDomainEventHandler : INotificationHandler<NewProductCreateDomainEvent>
{
    [AutoInject]
    private readonly ILogger<NewProductCreateDomainEventHandler> _logger;

    /// <inheritdoc />
    public async Task Handle(NewProductCreateDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("开始处理 ：{domainEventName} 领域事件", nameof(NewProductCreateDomainEvent));
        await Task.Delay(3000, cancellationToken);
        _logger.LogInformation("处理 ：{domainEventName} 领域事件完成", nameof(NewProductCreateDomainEvent));
    }
}