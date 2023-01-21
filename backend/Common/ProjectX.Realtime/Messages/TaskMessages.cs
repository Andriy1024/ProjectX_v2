namespace ProjectX.Realtime.Messages;

public abstract class TaskRealtimeMessage : IRealtimeMessage
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public bool Completed { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}

public class TaskCreatedMessage : TaskRealtimeMessage {}

public class TaskUpdatedMessage : TaskRealtimeMessage {}

public class TaskDeletedMessage : TaskRealtimeMessage {}