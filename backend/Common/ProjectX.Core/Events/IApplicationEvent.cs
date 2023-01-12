namespace ProjectX.Core.Events;

public interface IApplicationEvent : INotification
{
}

public interface IApplicationEventHandler<TDomainEvent> : INotificationHandler<TDomainEvent>
    where TDomainEvent : IApplicationEvent
{
}