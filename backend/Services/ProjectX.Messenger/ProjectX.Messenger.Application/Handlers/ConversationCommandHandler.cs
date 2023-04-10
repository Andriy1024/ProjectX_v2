using MediatR;
using ProjectX.Core;
using ProjectX.Core.Auth;
using ProjectX.Messenger.Application.UseCases;
using ProjectX.Messenger.Domain;
using ProjectX.Messenger.Persistence;

namespace ProjectX.Messenger.Application.Handlers;

public sealed class ConversationCommandHandler : 
    ICommandHandler<SendMessageCommand>,
    ICommandHandler<DeleteMessageCommand>,
    ICommandHandler<UpdateMessageCommand>
{
    private readonly IMessengerEventStore _eventStore;
    private readonly IUserContext _currentUser;

    public ConversationCommandHandler(IMessengerEventStore eventStore, IUserContext currentUser)
    {
        _eventStore = eventStore;
        _currentUser = currentUser;
    }

    public async Task<ResultOf<Unit>> Handle(SendMessageCommand command, CancellationToken cancellationToken)
    {
        var id = new ConversationId(_currentUser.Id, command.UserId);
        
        var conversation = await _eventStore.LoadAsync<ConversationAggregate>(id);
        
        conversation ??= ConversationAggregate.Start(_currentUser.Id, command.UserId);
        
        conversation.AddMessage(messageId: Guid.NewGuid(), _currentUser.Id, command.Content);
        
        await _eventStore.StoreAsync(conversation);
        
        return ResultOf<Unit>.Unit;
    }

    public async Task<ResultOf<Unit>> Handle(DeleteMessageCommand command, CancellationToken cancellationToken)
    {
        var conversation = await _eventStore.LoadAsync<ConversationAggregate>(command.ConversationId);
        
        if (conversation == null)
            return ApplicationError.NotFound(message: "Conversation not found.");

        conversation.DeleteMessage(command.MessageId, _currentUser.Id);
        
        await _eventStore.StoreAsync(conversation);
        
        return ResultOf<Unit>.Unit;
    }

    public async Task<ResultOf<Unit>> Handle(UpdateMessageCommand command, CancellationToken cancellationToken)
    {
        var conversation = await _eventStore.LoadAsync<ConversationAggregate>(command.ConversationId);

        if (conversation == null)
            return ApplicationError.NotFound(message: "Conversation not found.");

        conversation.UpdateMessage(command.MessageId, command.Content);
        
        await _eventStore.StoreAsync(conversation);
        
        return ResultOf<Unit>.Unit;
    }
}
