using Marten;
using Marten.Events;
using Marten.Events.Daemon.Resiliency;
using Marten.Events.Projections;
using Marten.Services.Json;
using ProjectX.Core;
using ProjectX.Core.Setup;
using ProjectX.Core.StartupTasks;
using ProjectX.Messenger.Domain;
using ProjectX.Messenger.Persistence;
using ProjectX.Messenger.Persistence.EventStore;
using ProjectX.Messenger.Persistence.Projections;
using Weasel.Core;

namespace ProjectX.Messenger.API;

public static class MartenSetup
{
    public static WebApplicationBuilder AddMarten(this WebApplicationBuilder app) 
    {
        var connectionString = app.Configuration.GetConnectionString("DbConnection");

        app.Services.AddMarten(o =>
        {
            o.Connection(connectionString.ThrowIfNullOrEmpty());

            o.AutoCreateSchemaObjects = AutoCreate.All;
            o.DatabaseSchemaName = "DocumentStore";
            o.Events.DatabaseSchemaName = "EventStore";

            // This is enough to tell Marten that the ConversationView document is persisted and needs schema objects
            o.Schema.For<ConversationView>()
                //.UseOptimisticConcurrency(true)
                .Identity(x => x.Id);

            o.Schema.For<UserConversationsView>()
                .Identity(x => x.UserId);

            o.Projections.Add<ConversationViewProjection>(ProjectionLifecycle.Async);
            o.Projections.Add<UserConversationsViewProjection>(ProjectionLifecycle.Async);

            // Lets Marten know that the event store is active
            o.Events.AddEventType(typeof(ConversationStarted));
            o.Events.AddEventType(typeof(MessageCreated));
            o.Events.AddEventType(typeof(MessageDeleted));
            o.Events.AddEventType(typeof(MessageUpdated));

            o.Events.StreamIdentity = StreamIdentity.AsString;

            // Opt into System.Text.Json serialization
            o.UseDefaultSerialization(
                serializerType: SerializerType.SystemTextJson,
                // Optionally override the enum storage
                enumStorage: EnumStorage.AsString,
                // Optionally override the member casing
                casing: Casing.CamelCase
            );
        })
       // Chained helper to replace the built in
       // session factory behavior
       .UseLightweightSessions()
       // Using the "Optimized artifact workflow" for Marten >= V5
       // sets up your Marten configuration based on your environment
       // See https://martendb.io/configuration/optimized_artifact_workflow.html
       .OptimizeArtifactWorkflow()
       // Optionally apply all database schema
       // changes on startup
       .ApplyAllDatabaseChangesOnStartup()
       .InitializeWith()
       // Turn on the async daemon in "Solo" mode
       .AddAsyncDaemon(DaemonMode.Solo);

        app.Services
            .AddOptions<ConnectionStrings>()
            .BindConfiguration(nameof(ConnectionStrings))
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Services
            .AddScoped<IStartupTask, DataBaseStartupTask>()
            .AddScoped<IMessengerEventStore, MartenEventStore>();

        return app;
    }
}
