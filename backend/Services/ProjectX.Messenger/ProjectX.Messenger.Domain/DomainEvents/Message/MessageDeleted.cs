namespace ProjectX.Messenger.Domain;

public sealed class MessageDeleted : IDomainEvent
{
    public string ConversationId { get; set; }

    public IEnumerable<int> Users { get; set; }

    public Guid MessageId { get; set; }

    public MessageDeleted() {}

    public MessageDeleted(ConversationId conversationId, IEnumerable<int> users, Guid messageId)
    {
        ConversationId = conversationId;
        Users = users;
        MessageId = messageId;
    }
}
