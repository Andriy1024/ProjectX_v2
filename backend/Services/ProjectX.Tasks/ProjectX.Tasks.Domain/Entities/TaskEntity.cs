using ProjectX.Core;

namespace ProjectX.Tasks.Domain.Entities;

public class TaskEntity : Entity<int>
{
    public string Name { get; set; }

    public string Description { get; set; }
}