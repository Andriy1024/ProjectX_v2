﻿namespace ProjectX.Realtime.Messages;

public class MessageCreatedMessage : IRealtimeMessage
{
    public long AuthorId { get; set; }
    public Guid MessageId { get; set; }
    public string ConversationId { get; set; }
    public string Content { get; set; }
    public DateTimeOffset CreatedAt { get; set; }

    public MessageCreatedMessage()
    {
    }

    public MessageCreatedMessage(long authorId, 
        Guid messageId, 
        string conversationId, 
        string content, 
        DateTimeOffset createdAt)
    {
        AuthorId = authorId;
        MessageId = messageId;
        ConversationId = conversationId;
        Content = content;
        CreatedAt = createdAt;
    }
}

public class MessageDeletedMessage : IRealtimeMessage 
{
    public Guid MessageId { get; set; }
    public string ConversationId { get; set; }

    public MessageDeletedMessage()
    {
    }

    public MessageDeletedMessage(Guid messageId, string conversationId)
    {
        MessageId = messageId;
        ConversationId = conversationId;
    }
}

public class MessageUpdatedMessage : IRealtimeMessage
{
    public Guid MessageId { get; set; }
    public string ConversationId { get; set; }
    public string Content { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }

    public MessageUpdatedMessage()
    {
    }

    public MessageUpdatedMessage(Guid messageId, string conversationId, string content, DateTimeOffset updatedAt)
    {
        MessageId = messageId;
        ConversationId = conversationId;
        Content = content;
        UpdatedAt = updatedAt;
    }
}
