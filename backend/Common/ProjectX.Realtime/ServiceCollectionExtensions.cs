using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectX.Realtime;

public static class RealtimeServiceCollectionExtensions
{
    public static IServiceCollection AddRealtimeServices(this IServiceCollection services)
        => services
            .AddScoped<IRealtimeTransactionContext, RealtimeTransactionContext>()
            .AddTransient<INotificationHandler<TransactionCommitedEvent>, TransactionCommitedRealtimeHandler>();
}
