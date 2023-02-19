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

public class TaskCreated : TaskRealtimeMessage {}

public class TaskUpdated : TaskRealtimeMessage {}

public class TaskDeleted : TaskRealtimeMessage {}