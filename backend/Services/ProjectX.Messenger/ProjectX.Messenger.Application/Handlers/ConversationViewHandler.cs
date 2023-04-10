using Marten;
using ProjectX.Core;
using ProjectX.Core.Auth;
using ProjectX.Messenger.Application.UseCases;
using ProjectX.Messenger.Domain;

namespace ProjectX.Messenger.Application.Handlers;

public sealed class ConversationViewHandler : 
    IQueryHandler<ConversationViewQuery, ConversationView>,
    IQueryHandler<UserConversationsQuery, IEnumerable<UserConversationsView.Conversation>>
{
    private readonly IDocumentSession _session;
    private readonly IUserContext _currentUser;
    
    public ConversationViewHandler(IDocumentSession session, IUserContext currentUser)
    {
        _session = session;
        _currentUser = currentUser;
    }

    public async Task<ResultOf<ConversationView>> Handle(ConversationViewQuery query, CancellationToken cancellationToken)
    {
        var id = new ConversationId(_currentUser.Id, query.UserId);

        var conversation = await _session
            .Query<ConversationView>()
            .FirstOrDefaultAsync(c => c.Id == id.Value, cancellationToken);

        return conversation != null
            ? conversation
            : ApplicationError.NotFound(message: "Conversation not found.");
    }

    public async Task<ResultOf<IEnumerable<UserConversationsView.Conversation>>> Handle(UserConversationsQuery query, CancellationToken cancellationToken)
    {
        var conversations = await _session
            .Query<UserConversationsView>()
            .FirstOrDefaultAsync(c => c.UserId == _currentUser.Id, cancellationToken);

        return conversations != null
            ? conversations.Conversations
            : Array.Empty<UserConversationsView.Conversation>();
    }
}
