namespace ProjectX.Tasks.Application.Contracts;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class TaskContarct
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.