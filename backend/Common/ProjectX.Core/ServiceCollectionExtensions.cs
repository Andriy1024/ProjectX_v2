using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using ProjectX.Core.Context;
using ProjectX.Core.Json.Implementations;
using ProjectX.Core.Json.Interfaces;
using ProjectX.Core.Validation;
using System.Reflection;

namespace ProjectX.Core;

public static class ServiceCollectionExtensions
{
    public static WebApplicationBuilder AddCoreServices(this WebApplicationBuilder app, params Assembly[] assemblies)
    {
        app.Services.AddCoreServices(assemblies);

        return app;
    }

    public static IServiceCollection AddCoreServices(this IServiceCollection services, params Assembly[] assemblies)
    {
        services
            .AddMediatR(assemblies)
            .AddTransient<IEventDispatcher, EventDispatcher>()
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>))
            .AddAutoMapper(assemblies)
            .AddCoreSerialization()
            .AddHttpContextAccessor()
            .AddContexts();

        return services;
    }

    public static IHealthChecksBuilder AddCoreHealthChecks(this IServiceCollection services)
    {
        return services
            .AddHealthChecks()
            .AddCheck("self", () => HealthCheckResult.Healthy());
    }

    public static IServiceCollection AddCoreSerialization(this IServiceCollection services)
        => services.AddSingleton<IApplicationJsonSerializer, ApplicationJsonSerializer>();
}
