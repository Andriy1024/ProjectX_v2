﻿namespace ProjectX.Core;

public interface IEntity<TKey> : IEntity
{
    TKey Id { get; }
}

public interface IEntity
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void AddDomainEvent(IDomainEvent eventItem);

    void RemoveDomainEvent(IDomainEvent eventItem);

    void ClearDomainEvents();

    bool IsTransient();
}