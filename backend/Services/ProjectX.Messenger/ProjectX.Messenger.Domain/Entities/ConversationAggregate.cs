﻿namespace ProjectX.Messenger.Domain;

public sealed class ConversationAggregate : EventSourcedAggregate
{
    public ConversationId Id { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public IReadOnlyCollection<int> Users => _users;

    private readonly HashSet<int> _users = new HashSet<int>(2);

    public IReadOnlyCollection<MessageEntity> Messages => _messages;

    private readonly List<MessageEntity> _messages = new List<MessageEntity>();

    public ConversationAggregate() { }

    private ConversationAggregate(ConversationId id, int firstUser, int secondUser) 
    {
        var @event = new ConversationStarted(id, DateTimeOffset.UtcNow, new[] { firstUser, secondUser });
        
        Apply(@event);
    }

    public static ConversationAggregate Start(int firstUser, int secondUser) 
    {
        if(firstUser == secondUser) 
        {
            throw new BadDataException(ErrorCode.BadData, $"Invalid participant ids: {firstUser} == {secondUser}.");
        }

        return new ConversationAggregate(new ConversationId(firstUser, secondUser), firstUser, secondUser);
    }

    public override string GetId() => Id;

    public void AddMessage(Guid messageId, int author, string content) 
    {
        if(!IsBelongToConversation(author)) 
        {
            throw new InvalidPermissionException(ErrorCode.InvalidPermission, "The author doesn't belong to the conversation.");
        }

        var @event = new MessageCreated(messageId: messageId, 
                                        conversationId: Id,
                                        users: _users,
                                        authorId: author, 
                                        content: content, 
                                        createdAt: DateTimeOffset.UtcNow);

        Apply(@event);
    }

    public void UpdateMessage(Guid id, string content) 
    {
        content.ThrowIfNullOrEmpty();

        var message = GetMessageRequired(id);

        var @event = new MessageUpdated(conversationId: Id,
                                        users: _users,
                                        messageId: message.Id,
                                        content: content,
                                        updatedAt: DateTimeOffset.UtcNow);

        Apply(@event);
    }

    public void DeleteMessage(Guid id, int deleteBy) 
    {
        var message = GetMessageRequired(id);
        
        if(message.AuthorId != deleteBy) 
        {
            throw new InvalidPermissionException(ErrorCode.InvalidPermission, "Message can be deleted only by the owner.");
        }

        var @event = new MessageDeleted(conversationId: Id, users: _users, messageId: message.Id);
        
        Apply(@event);
    }

    private bool IsBelongToConversation(int userId) => _users.Contains(userId);
    
    private MessageEntity GetMessageRequired(Guid id) 
    {
        var message = _messages.FirstOrDefault(m => m.Id == id);

        if (message == null)
        {
            throw new NotFoundException(ErrorCode.ConversationMessageNotFound);
        }

        return message;
    }

    private void When(ConversationStarted @event) 
    {
        var firstUser = @event.Users.First();
        var secondUser = @event.Users.Last();
        Id = new ConversationId(firstUser, secondUser);
        _users.Add(firstUser);
        _users.Add(secondUser);
        CreatedAt = @event.CreatedAt;
    }

    private void When(MessageCreated @event) 
    {
        var message = new MessageEntity(conversationId: Id, 
                                        id: @event.MessageId,
                                        authorId: @event.AuthorId,
                                        content: @event.Content,
                                        createdAt: @event.CreatedAt);

        _messages.Add(message);
    }

    private void When(MessageUpdated @event) 
    {
        var message = GetMessageRequired(@event.MessageId);
       
        message.Update(@event.Content, @event.UpdatedAt);
    }

    private void When(MessageDeleted @event) 
    {
        var message = GetMessageRequired(@event.MessageId);

        _messages.Remove(message);
    }

    protected override void When(IDomainEvent @event) 
    {
        switch (@event)
        {
            case ConversationStarted e:
                When(e);
                break;
            case MessageCreated e:
                When(e);
                break;
            case MessageUpdated e:
                When(e);
                break;
            case MessageDeleted e:
                When(e);
                break;
            default:
                throw new ArgumentOutOfRangeException($"Unknown event type: {@event.GetType().Name}.");
        }
    }
}
