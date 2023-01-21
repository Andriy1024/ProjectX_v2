namespace ProjectX.Realtime.Messages;

public class TaskCreatedMessage : IRealtimeMessage
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string? Description { get; set; }

    public bool Completed { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}