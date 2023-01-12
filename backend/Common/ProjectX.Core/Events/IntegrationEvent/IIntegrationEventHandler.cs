namespace ProjectX.Core.Events.IntegrationEvent;

public interface IIntegrationEventHandler<TEvent> : IRequestHandler<TEvent>
        where TEvent : IIntegrationEvent
{
}