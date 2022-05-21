using ProjectX.Core;

namespace ProjectX.Tasks.Domain.Entities;

public class TaskEntity : Entity<int>
{
    public override int Id { get; protected set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    /// <summary>
    /// For EF
    /// </summary>
    private TaskEntity() {}
    
    public TaskEntity(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public void Update(string name, string? description) 
    {
        Name = name;
        Description = description;
    }

    public static TaskEntity Create(string name, string? description)
        => new TaskEntity(name, description);
}