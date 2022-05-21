﻿using ProjectX.Core;

namespace ProjectX.Tasks.Domain.Entities;

public class TaskEntity : Entity<int>
{
    public override int Id { get; protected set; }

    public string Name { get; private set; }

    public string? Description { get; private set; }

    /// <summary>
    /// For EF
    /// </summary>
#pragma warning disable CS8618
    private TaskEntity() {}
#pragma warning restore CS8618

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