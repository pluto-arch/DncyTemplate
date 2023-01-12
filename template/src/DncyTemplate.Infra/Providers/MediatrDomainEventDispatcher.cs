using DncyTemplate.Domain.Infra;
using MediatR;

namespace DncyTemplate.Infra.Providers;

public class MediatrDomainEventDispatcher : IDomainEventDispatcher
{
    private readonly ILogger<MediatrDomainEventDispatcher> _log;
    private readonly IMediator _mediator;

    public MediatrDomainEventDispatcher(IMediator mediator, ILogger<MediatrDomainEventDispatcher> log)
    {
        _mediator = mediator;
        _log = log;
    }

    public async Task Dispatch(INotification domainEvent, CancellationToken cancellationToken = default)
    {
        await _mediator.Publish(domainEvent, cancellationToken);
    }
}