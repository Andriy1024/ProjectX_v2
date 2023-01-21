namespace ProjectX.Realtime.Messages;

public abstract class BookmarkMessage : IRealtimeMessage
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string URL { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}

public class BookmarkCreated : BookmarkMessage { }

public class BookmarkUpdated : BookmarkMessage { }

public class BookmarkDeleted : BookmarkMessage { }