using ProjectX.Core.Abstractions;

namespace ProjectX.Core.Events.IntegrationEvent;

/// <summary>
/// The interface represents an event that is sent to message broker (RabbitMq).
/// </summary>
public interface IIntegrationEvent : IRequest
{
}

/// <summary>
/// Persistent enevts are saved to database.
/// </summary>
public interface IPersistentIntegrationEvent : IIntegrationEvent, IHasTransaction
{
    public Guid Id { get; set; }
}

/// <summary>
/// Transient events are not persistent in database.
/// </summary>
public interface ITransientIntegrationEvent : IIntegrationEvent
{
}