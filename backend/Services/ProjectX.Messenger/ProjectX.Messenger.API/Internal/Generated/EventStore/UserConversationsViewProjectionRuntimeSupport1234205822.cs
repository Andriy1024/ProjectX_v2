// <auto-generated/>
#pragma warning disable
using Marten;
using Marten.Events.Aggregation;
using Marten.Internal.Storage;
using ProjectX.Messenger.Persistence.Projections;
using System;
using System.Linq;

namespace Marten.Generated.EventStore
{
    // START: UserConversationsViewProjectionLiveAggregation1234205822
    public class UserConversationsViewProjectionLiveAggregation1234205822 : Marten.Events.Aggregation.AsyncLiveAggregatorBase<ProjectX.Messenger.Domain.UserConversationsView>
    {
        private readonly ProjectX.Messenger.Persistence.Projections.UserConversationsViewProjection _userConversationsViewProjection;

        public UserConversationsViewProjectionLiveAggregation1234205822(ProjectX.Messenger.Persistence.Projections.UserConversationsViewProjection userConversationsViewProjection)
        {
            _userConversationsViewProjection = userConversationsViewProjection;
        }


        public System.Action<ProjectX.Messenger.Domain.UserConversationsView, ProjectX.Messenger.Domain.MessageCreated> ProjectEvent1 {get; set;}

        public System.Action<ProjectX.Messenger.Domain.UserConversationsView, ProjectX.Messenger.Domain.MessageUpdated> ProjectEvent2 {get; set;}

        public System.Func<Marten.IQuerySession, ProjectX.Messenger.Domain.UserConversationsView, ProjectX.Messenger.Domain.MessageDeleted, System.Threading.Tasks.Task> ProjectEvent3 {get; set;}


        public override async System.Threading.Tasks.ValueTask<ProjectX.Messenger.Domain.UserConversationsView> BuildAsync(System.Collections.Generic.IReadOnlyList<Marten.Events.IEvent> events, Marten.IQuerySession session, ProjectX.Messenger.Domain.UserConversationsView snapshot, System.Threading.CancellationToken cancellation)
        {
            if (!events.Any()) return null;
            ProjectX.Messenger.Domain.UserConversationsView userConversationsView = null;
            snapshot ??= Create(events[0], session);
            foreach (var @event in events)
            {
                snapshot = await Apply(@event, snapshot, session, cancellation);
            }

            return snapshot;
        }


        public ProjectX.Messenger.Domain.UserConversationsView Create(Marten.Events.IEvent @event, Marten.IQuerySession session)
        {
            return new ProjectX.Messenger.Domain.UserConversationsView();
        }


        public async System.Threading.Tasks.ValueTask<ProjectX.Messenger.Domain.UserConversationsView> Apply(Marten.Events.IEvent @event, ProjectX.Messenger.Domain.UserConversationsView aggregate, Marten.IQuerySession session, System.Threading.CancellationToken cancellation)
        {
            switch (@event)
            {
                case Marten.Events.IEvent<ProjectX.Messenger.Domain.MessageDeleted> event_MessageDeleted9:
                    await _userConversationsViewProjection.Apply(session, aggregate, event_MessageDeleted9.Data).ConfigureAwait(false);
                    await ProjectEvent3.Invoke(session, aggregate, event_MessageDeleted9.Data).ConfigureAwait(false);
                    break;
                case Marten.Events.IEvent<ProjectX.Messenger.Domain.MessageCreated> event_MessageCreated10:
                    aggregate.Apply(event_MessageCreated10.Data);
                    ProjectEvent1.Invoke(aggregate, event_MessageCreated10.Data);
                    break;
                case Marten.Events.IEvent<ProjectX.Messenger.Domain.MessageView> event_MessageView11:
                    aggregate.Apply(event_MessageView11.Data);
                    break;
                case Marten.Events.IEvent<ProjectX.Messenger.Domain.MessageUpdated> event_MessageUpdated12:
                    aggregate.Apply(event_MessageUpdated12.Data);
                    ProjectEvent2.Invoke(aggregate, event_MessageUpdated12.Data);
                    break;
            }

            return aggregate;
        }

    }

    // END: UserConversationsViewProjectionLiveAggregation1234205822
    
    
    // START: UserConversationsViewProjectionInlineHandler1234205822
    public class UserConversationsViewProjectionInlineHandler1234205822 : Marten.Events.Aggregation.CrossStreamAggregationRuntime<ProjectX.Messenger.Domain.UserConversationsView, int>
    {
        private readonly Marten.IDocumentStore _store;
        private readonly Marten.Events.Aggregation.IAggregateProjection _projection;
        private readonly Marten.Events.Aggregation.IEventSlicer<ProjectX.Messenger.Domain.UserConversationsView, int> _slicer;
        private readonly Marten.Internal.Storage.IDocumentStorage<ProjectX.Messenger.Domain.UserConversationsView, int> _storage;
        private readonly ProjectX.Messenger.Persistence.Projections.UserConversationsViewProjection _userConversationsViewProjection;

        public UserConversationsViewProjectionInlineHandler1234205822(Marten.IDocumentStore store, Marten.Events.Aggregation.IAggregateProjection projection, Marten.Events.Aggregation.IEventSlicer<ProjectX.Messenger.Domain.UserConversationsView, int> slicer, Marten.Internal.Storage.IDocumentStorage<ProjectX.Messenger.Domain.UserConversationsView, int> storage, ProjectX.Messenger.Persistence.Projections.UserConversationsViewProjection userConversationsViewProjection) : base(store, projection, slicer, storage)
        {
            _store = store;
            _projection = projection;
            _slicer = slicer;
            _storage = storage;
            _userConversationsViewProjection = userConversationsViewProjection;
        }


        public System.Action<ProjectX.Messenger.Domain.UserConversationsView, ProjectX.Messenger.Domain.MessageCreated> ProjectEvent1 {get; set;}

        public System.Action<ProjectX.Messenger.Domain.UserConversationsView, ProjectX.Messenger.Domain.MessageUpdated> ProjectEvent2 {get; set;}

        public System.Func<Marten.IQuerySession, ProjectX.Messenger.Domain.UserConversationsView, ProjectX.Messenger.Domain.MessageDeleted, System.Threading.Tasks.Task> ProjectEvent3 {get; set;}


        public override async System.Threading.Tasks.ValueTask<ProjectX.Messenger.Domain.UserConversationsView> ApplyEvent(Marten.IQuerySession session, Marten.Events.Projections.EventSlice<ProjectX.Messenger.Domain.UserConversationsView, int> slice, Marten.Events.IEvent evt, ProjectX.Messenger.Domain.UserConversationsView aggregate, System.Threading.CancellationToken cancellationToken)
        {
            switch (evt)
            {
                case Marten.Events.IEvent<ProjectX.Messenger.Domain.MessageView> event_MessageView15:
                    aggregate ??= new ProjectX.Messenger.Domain.UserConversationsView();
                    aggregate.Apply(event_MessageView15.Data);
                    return aggregate;
                case Marten.Events.IEvent<ProjectX.Messenger.Domain.MessageUpdated> event_MessageUpdated16:
                    aggregate ??= new ProjectX.Messenger.Domain.UserConversationsView();
                    ProjectEvent2.Invoke(aggregate, event_MessageUpdated16.Data);
                    return aggregate;
                case Marten.Events.IEvent<ProjectX.Messenger.Domain.MessageCreated> event_MessageCreated14:
                    aggregate ??= new ProjectX.Messenger.Domain.UserConversationsView();
                    ProjectEvent1.Invoke(aggregate, event_MessageCreated14.Data);
                    return aggregate;
                case Marten.Events.IEvent<ProjectX.Messenger.Domain.MessageDeleted> event_MessageDeleted13:
                    aggregate ??= new ProjectX.Messenger.Domain.UserConversationsView();
                    await ProjectEvent3.Invoke(session, aggregate, event_MessageDeleted13.Data).ConfigureAwait(false);
                    return aggregate;
            }

            return aggregate;
        }


        public ProjectX.Messenger.Domain.UserConversationsView Create(Marten.Events.IEvent @event, Marten.IQuerySession session)
        {
            return new ProjectX.Messenger.Domain.UserConversationsView();
        }

    }

    // END: UserConversationsViewProjectionInlineHandler1234205822
    
    
}

