namespace ProjectX.Core;

public interface IDomainEvent : INotification
{
}

public interface IDomainEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}