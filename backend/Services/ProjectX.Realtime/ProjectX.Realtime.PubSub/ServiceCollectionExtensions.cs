using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using ProjectX.Realtime.PubSub.MessageBus;

namespace ProjectX.Realtime.PubSub;

public static class ServiceCollectionExtensions
{
    // <summary>
    /// Adds the in-memory subscription provider to the GraphQL configuration.
    /// </summary>
    /// <param name="builder">
    /// The GraphQL configuration builder.
    /// </param>
    /// <param name="options">
    /// The subscription provider options.
    /// </param>
    /// <returns>
    /// Returns the GraphQL configuration builder for configuration chaining.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> is <c>null</c>.
    /// </exception>
    public static IServiceCollection AddInMemorySubscriptions(
        this IServiceCollection services,
        SubscriptionOptions? options = null)
    {
        if (services is null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.TryAddSingleton(options ?? new SubscriptionOptions());
        services.TryAddSingleton<InMemoryPubSub>();
        services.TryAddSingleton<ITopicEventSender>(
            sp => sp.GetRequiredService<InMemoryPubSub>());
        services.TryAddSingleton<ITopicEventReceiver>(
            sp => sp.GetRequiredService<InMemoryPubSub>());

        return services;
    }
}
