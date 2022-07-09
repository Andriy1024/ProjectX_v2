namespace ProjectX.Core;

public class EntityCreated<TEntity> : IDomainEvent
{
    public TEntity Entity { get; }

    public EntityCreated(TEntity entity)
    {
        Entity = entity;
    }
}

public class EntityUpdated<TEntity> : IDomainEvent
{
    public TEntity Entity { get; }

    public EntityUpdated(TEntity entity)
    {
        Entity = entity;
    }
}

public class EntityDeleted<TEntity> : IDomainEvent
{
    public TEntity Entity { get; }

    public EntityDeleted(TEntity entity)
    {
        Entity = entity;
    }
}