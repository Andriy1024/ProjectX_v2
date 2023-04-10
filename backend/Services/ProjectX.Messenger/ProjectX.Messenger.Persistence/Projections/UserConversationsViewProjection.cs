using Marten;
using Marten.Events.Projections;
using ProjectX.Messenger.Domain;

namespace ProjectX.Messenger.Persistence.Projections;

public class UserConversationsViewProjection : MultiStreamProjection<UserConversationsView, int>
{
    public UserConversationsViewProjection()
    {
        // This is just specifying the aggregate document id
        // per event type. This assumes that each event
        // applies to only one aggregated view document
        Identities<MessageCreated>((ev) => ev.Users.ToList());
        Identities<MessageUpdated>((ev) => ev.Users.ToList());
        Identities<MessageDeleted>((ev) => ev.Users.ToList());

        ProjectEvent<MessageCreated>((view, @event) => view.Apply(@event));
        ProjectEvent<MessageUpdated>((view, @event) => view.Apply(@event));

        ProjectEventAsync<MessageDeleted>(async (session, view, @event) => await Apply(session, view, @event));
    }

    public async Task Apply(IQuerySession session, UserConversationsView view, MessageDeleted @event)
    {
        var conversation = await session.Query<ConversationView>().FirstOrDefaultAsync(u => u.Id == @event.ConversationId);

        if (conversation == null)
        {
            view.DeleteConversation(@event.ConversationId);
            
            return;
        }

        var lastMessage = conversation.Messages.LastOrDefault(x => x.Id != @event.MessageId);

        if (lastMessage == null)
        {
            view.DeleteConversation(conversation.Id);
            
            return;
        }

        view.Apply(lastMessage);
    }
}