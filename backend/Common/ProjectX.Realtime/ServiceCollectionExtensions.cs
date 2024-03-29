﻿using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectX.Realtime;

public static class RealtimeServiceCollectionExtensions
{
    public static WebApplicationBuilder AddRealtimeServices(this WebApplicationBuilder app) 
    {
        app.Services.AddRealtimeServices();

        return app;
    }

    public static IServiceCollection AddRealtimeServices(this IServiceCollection services)
        => services
            .AddScoped<IRealtimeTransactionContext, RealtimeTransactionContext>()
            .AddTransient<INotificationHandler<TransactionCommitedEvent>, TransactionCommitedRealtimeHandler>();
}
