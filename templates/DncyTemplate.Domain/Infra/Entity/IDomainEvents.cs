using MediatR;

namespace DncyTemplate.Domain.Infra;

public interface IDomainEvents
{
    IReadOnlyCollection<INotification> DomainEvents { get; }

    void AddDomainEvent(INotification eventItem);

    void RemoveDomainEvent(INotification eventItem);

    void ClearDomainEvents();
}