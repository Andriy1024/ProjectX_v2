namespace ProjectX.Core;

public abstract class EntityDomainEvent<TEntity> : IDomainEvent
{
    public TEntity Entity { get; }

    public EntityDomainEvent(TEntity entity)
    {
        Entity = entity;
    }
}

public class EntityCreated<TEntity> : EntityDomainEvent<TEntity>
{
    public EntityCreated(TEntity entity)
        : base(entity)
    {
    }
}

public class EntityUpdated<TEntity> : EntityDomainEvent<TEntity>
{
    public EntityUpdated(TEntity entity)
        :base(entity)
    {
    }
}

public class EntityDeleted<TEntity> : EntityDomainEvent<TEntity>
{
    public EntityDeleted(TEntity entity)
        : base(entity)
    {
    }
}