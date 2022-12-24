using ProjectX.Core;

namespace ProjectX.Dashboard.Domain.Entities;

public sealed class TaskEntity : Entity<int>
{
    public override int Id { get; protected set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    public bool Completed { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset UpdatedAt { get; private set; }

    #pragma warning disable CS8618
    /// <summary>
    /// For EF
    /// </summary>
    private TaskEntity() {}
    #pragma warning restore CS8618

    public TaskEntity(string name, string? description)
    {
        Name = name;
        Description = description;
        CreatedAt = DateTimeOffset.UtcNow;
        UpdatedAt = DateTimeOffset.UtcNow;
        Completed= false;

        AddDomainEvent(new EntityCreated<TaskEntity>(this));
    }

    public void Update(string name, string? description, bool completed)
    {
        Name = name;
        Description = description;
        Completed = completed;
        UpdatedAt = DateTimeOffset.UtcNow;

        AddDomainEvent(new EntityDeleted<TaskEntity>(this));
    }

    public void Remove() 
    {
        AddDomainEvent(new EntityDeleted<TaskEntity>(this));
    }

    public static TaskEntity Create(string name, string? description)
        => new TaskEntity(name, description);
}