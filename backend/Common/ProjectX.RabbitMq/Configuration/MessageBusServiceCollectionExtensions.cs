using Microsoft.Extensions.DependencyInjection;
using ProjectX.RabbitMq.Implementations;
using Microsoft.Extensions.Configuration;
using ProjectX.RabbitMq.Publisher;
using ProjectX.RabbitMq.Subscriber;
using Microsoft.AspNetCore.Builder;

namespace ProjectX.RabbitMq.Configuration;

public static class MessageBusServiceCollectionExtensions
{
    public static WebApplicationBuilder AddRabbitMqMessageBus(this WebApplicationBuilder app)
    {
        app.Services.AddRabbitMqMessageBus<MessageDispatcher>(app.Configuration);

        return app;
    }

    public static IServiceCollection AddRabbitMqMessageBus(this IServiceCollection services, IConfiguration configuration)
        => services.AddRabbitMqMessageBus<MessageDispatcher>(configuration);

    public static IServiceCollection AddRabbitMqMessageBus<TMessageDispatcher>(this IServiceCollection services, IConfiguration configuration)
        where TMessageDispatcher : class, IMessageDispatcher
    {
        return services
            .Configure<RabbitMqConfiguration>(configuration.GetSection("RabbitMq"))
            .AddSingleton<IMessageBroker, MessageBroker>()
            .AddSingleton<IRabbitMqSubscriber, RabbitMqSubscriber>()
            .AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>()
            .AddSingleton<IMessageSerializer, DefaultMessageSerializer>()
            .AddSingleton<IMessageDispatcher, TMessageDispatcher>()
            .AddSingleton<IRabbitMqConnectionService, RabbitMqConnectionService>();
    }
}