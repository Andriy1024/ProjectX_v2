using ProjectX.Core.Events;
using ProjectX.Core.Observability;

namespace ProjectX.Core;

public interface IEventDispatcher
{
    Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IApplicationEvent;

    Task DispatchAsync(IEnumerable<IApplicationEvent> domainEvents);
}

public sealed class EventDispatcher : IEventDispatcher
{
    private readonly IMediator _mediator;
    private readonly ITracer _tracer; 

    public EventDispatcher(IMediator mediator, ITracer tracer)
    {
        _mediator = mediator;
        _tracer = tracer;
    }

    public async Task DispatchAsync(IEnumerable<IApplicationEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await DispatchAsync(domainEvent);
        }
    }

    public Task DispatchAsync<TEvent>(TEvent domainEvent) where TEvent : IApplicationEvent
    {
        return _tracer.Trace(domainEvent.GetType().Name, () => _mediator.Publish(domainEvent));
    }
}