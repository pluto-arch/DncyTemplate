using DncyTemplate.Application.IntegrationEvents.Events;
using DncyTemplate.Application.IntegrationEvents.IntegrationEventbox;
using DncyTemplate.Domain.DomainEvents.Product;

namespace DncyTemplate.Application.DomainEventHandlers.Product;

[AutoResolveDependency]
public partial class NewProductCreateDomainEventHandler : INotificationHandler<NewProductCreateDomainEvent>
{
    [AutoInject]
    private readonly ILogger<NewProductCreateDomainEventHandler> _logger;

    [AutoInject]
    private readonly IntegrationEventBoxService _integrationEventBox;

    /// <inheritdoc />
    public async Task Handle(NewProductCreateDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("开始处理 ：{domainEventName} 领域事件", nameof(NewProductCreateDomainEvent));
        await Task.Delay(20, cancellationToken);
        _logger.LogInformation("处理 ：{domainEventName} 领域事件完成", nameof(NewProductCreateDomainEvent));

        notification.Prod.Remark = $"{notification.Prod.Remark}:领域事件处理器";

        // 同时保存集成事件和业务数据
        var intEv = new ProductCreatedIntegrationEvent(notification.Prod.Id);
        await _integrationEventBox.SaveEventAndChangesAsync(intEv);
        await _integrationEventBox.PublishThroughEventBusAsync(intEv);
    }
}