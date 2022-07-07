
namespace DncyTemplate.Domain.Infra;


/// <summary>
/// 空领域事件触发器
/// </summary>
public class NullDomainEventDispatcher : IDomainEventDispatcher
{
    public static IDomainEventDispatcher Instance { get; } = new NullDomainEventDispatcher();

    /// <inheritdoc />
    public Task Dispatch(INotification domainEvent, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}