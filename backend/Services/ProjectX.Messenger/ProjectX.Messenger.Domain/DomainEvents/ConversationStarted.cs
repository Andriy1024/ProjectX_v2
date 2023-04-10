namespace ProjectX.Messenger.Domain;

public sealed class ConversationStarted : IDomainEvent
{
    public string Id { get; set; }

    public IEnumerable<int> Users { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    private ConversationStarted() {}

    public ConversationStarted(string id, DateTimeOffset createdAt, IEnumerable<int> users)
    {
        Id = id;
        CreatedAt = createdAt;
        Users = users;
    }
}
