using ProjectX.Core.Events;

namespace ProjectX.Core;

public interface IDomainEvent : IApplicationEvent
{
}

public interface IDomainEventHandler<TDomainEvent> : IApplicationEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
}