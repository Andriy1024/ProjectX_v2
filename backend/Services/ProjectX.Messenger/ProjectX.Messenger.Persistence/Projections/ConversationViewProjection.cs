using Marten.Events.Projections;
using ProjectX.Messenger.Domain;

namespace ProjectX.Messenger.Persistence.Projections;

public class ConversationViewProjection : MultiStreamProjection<ConversationView, string>
{
    public ConversationViewProjection()
    {
        Identity<ConversationStarted>((ev) => ev.Id);
        Identity<MessageCreated>((ev) => ev.ConversationId);
        Identity<MessageUpdated>((ev) => ev.ConversationId);
        Identity<MessageDeleted>((ev) => ev.ConversationId);

        ProjectEvent<ConversationStarted>((view, @event) => view.Apply(@event));
        ProjectEvent<MessageCreated>((view, @event) => view.Apply(@event));
        ProjectEvent<MessageUpdated>((view, @event) => view.Apply(@event));
        ProjectEvent<MessageDeleted>((view, @event) => view.Apply(@event));
    }
}