namespace ProjectX.Messenger.Domain;

/// <summary>
/// Conversation message entity.
/// </summary>
public sealed class MessageEntity
{
    public Guid Id { get; private set; }

    public ConversationId ConversationId { get; private set; }

    public int AuthorId { get; private set; }

    public string Content { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public DateTimeOffset? UpdatedAt { get; private set; }

    private MessageEntity() { }

    public MessageEntity(ConversationId conversationId, Guid id, int authorId, string content, DateTimeOffset createdAt)
    {
        ConversationId = conversationId;
        Id = id;
        AuthorId = authorId;
        Content = content;
        CreatedAt = createdAt;
    }

    public void Update(string content, DateTimeOffset updatedAt) 
    {
        Content = content;
        UpdatedAt = updatedAt;
    }
}
