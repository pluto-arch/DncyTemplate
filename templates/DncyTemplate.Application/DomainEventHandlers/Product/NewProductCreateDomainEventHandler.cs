using DncyTemplate.Domain.DomainEvents.Product;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DncyTemplate.Application.DomainEventHandlers.Product;

public class NewProductCreateDomainEventHandler : INotificationHandler<NewProductCreateDomainEvent>
{
    private readonly ILogger<NewProductCreateDomainEventHandler> _logger;

    public NewProductCreateDomainEventHandler(ILogger<NewProductCreateDomainEventHandler> logger)
    {
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task Handle(NewProductCreateDomainEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("开始处理 ：{domainEventName} 领域事件", nameof(NewProductCreateDomainEvent));
        await Task.Delay(3000, cancellationToken);
        _logger.LogInformation("处理 ：{domainEventName} 领域事件完成", nameof(NewProductCreateDomainEvent));
    }
}