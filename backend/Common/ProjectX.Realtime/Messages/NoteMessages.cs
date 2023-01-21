namespace ProjectX.Realtime.Messages;

public abstract class NoteMessage : IRealtimeMessage
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}

public class NoteCreated : NoteMessage { }

public class NoteUpdated : NoteMessage { }

public class NoteDeleted : NoteMessage { }