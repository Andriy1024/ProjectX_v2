using Microsoft.Extensions.DependencyInjection;
using ProjectX.Core.Realtime.Abstractions;
using ProjectX.Dashboard.Application.Handlers.DomainEvents;
using ProjectX.Realtime.Messages;

namespace ProjectX.Dashboard.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainEventsHandlers(this IServiceCollection services) 
    {
        return services
            // Task handlers
            .AddDomainEventsHandlers<TaskEntity, EntityCreated<TaskEntity>, TaskCreatedMessage>()
            .AddDomainEventsHandlers<TaskEntity, EntityUpdated<TaskEntity>, TaskUpdatedMessage>()
            .AddDomainEventsHandlers<TaskEntity, EntityDeleted<TaskEntity>, TaskDeletedMessage>()
            // Notes handlers
            .AddDomainEventsHandlers<NoteEntity, EntityCreated<NoteEntity>, NoteCreated>()
            .AddDomainEventsHandlers<NoteEntity, EntityUpdated<NoteEntity>, NoteUpdated>()
            .AddDomainEventsHandlers<NoteEntity, EntityDeleted<NoteEntity>, NoteDeleted>()
            // Bookmark handlers
            .AddDomainEventsHandlers<BookmarkEntity, EntityCreated<BookmarkEntity>, BookmarkCreated>()
            .AddDomainEventsHandlers<BookmarkEntity, EntityUpdated<BookmarkEntity>, BookmarkUpdated>()
            .AddDomainEventsHandlers<BookmarkEntity, EntityDeleted<BookmarkEntity>, BookmarkDeleted>();
    }

    public static IServiceCollection AddDomainEventsHandlers<TEntity, TEvent, TMessage>(this IServiceCollection services)
        where TEvent : EntityDomainEvent<TEntity>
        where TMessage : IRealtimeMessage
        => services.AddTransient<INotificationHandler<TEvent>, RealtimeEntityDomainEventHandler<TEntity, TEvent, TMessage>>();
}
