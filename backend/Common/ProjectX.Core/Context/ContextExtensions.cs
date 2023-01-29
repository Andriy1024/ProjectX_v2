using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using ProjectX.Core.Context.HTTP;
using System.Diagnostics;

namespace ProjectX.Core.Context;

public static class ContextExtensions
{
    private const string CorrelationIdKey = "correlation-id";

    public static IServiceCollection AddContexts(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddSingleton<IContextProvider, ContextProvider>();
        services.AddSingleton<IContextAccessor, ContextAccessor>();
        services.AddTransient<ContextHttpHandler>();

        return services;
    }

    public static IHttpClientBuilder AddContextHandler(this IHttpClientBuilder builder)
        => builder.AddHttpMessageHandler<ContextHttpHandler>();

    public static IApplicationBuilder UseCorrelationContext(this IApplicationBuilder app)
        => app.Use((ctx, next) =>
        {
            if (!ctx.Request.Headers.TryGetValue(CorrelationIdKey, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString("N");
            }

            Activity.Current?.SetTag("correlation_id", correlationId);
            ctx.Response.Headers.Add(CorrelationIdKey, correlationId);
            ctx.Items.Add(CorrelationIdKey, correlationId);
            
            return next();
        });

    public static string? GetCorrelationId(this HttpContext context)
        => context.Items.TryGetValue(CorrelationIdKey, out var correlationId) ? correlationId?.ToString() : null;
}