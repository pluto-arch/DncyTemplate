using DncyTemplate.Domain.Infra;

using MediatR;

namespace DncyTemplate.Infra.Providers;

public class MediatrDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<MediatrDomainEventDispatcher> _log;
    private readonly IServiceProvider _serviceProvider;

    public MediatrDomainEventDispatcher(IServiceProvider serviceProvider, ILogger<MediatrDomainEventDispatcher> log)
    {
        _serviceProvider = serviceProvider;
        _log = log;
    }

    public async Task Dispatch(INotification domainEvent, CancellationToken cancellationToken = default)
    {
        using var scope = _serviceProvider.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        _log.LogDebug("Dispatching Domain Event as MediatR notification.  EventType: {eventType}", domainEvent.GetType());
        await mediator.Publish(domainEvent, cancellationToken);
    }
}