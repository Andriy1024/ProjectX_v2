using ProjectX.Core.Observability;

namespace ProjectX.Core;

public interface IEventDispatcher
{
    Task DispatchAsync(IDomainEvent domainEvent);

    Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents);
}

public class EventDispatcher : IEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ITracer _tracer; 

    public EventDispatcher(IMediator mediator, ITracer tracer)
    {
        _mediator = mediator;
        _tracer = tracer;
    }

    public async Task DispatchAsync(IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await DispatchAsync(domainEvent);
        }
    }

    public Task DispatchAsync(IDomainEvent domainEvent)
    {
        return _tracer.Trace(domainEvent.GetType().Name, () => _mediator.Publish(domainEvent));
    }
}