using MediatR;

namespace DncyTemplate.Domain.Infra;

/// <summary>
/// 领域事件触发器
/// </summary>
public interface IDomainEventDispatcher
{
    Task Dispatch(INotification domainEvent, CancellationToken cancellationToken = default);
}