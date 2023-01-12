using Microsoft.Extensions.DependencyInjection;
using ProjectX.Core.Json.Implementations;
using ProjectX.Core.Json.Interfaces;

namespace ProjectX.Core.Json;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreSerialization(this IServiceCollection services)
    {
        return services
            .AddSingleton<IApplicationJsonSerializer, ApplicationJsonSerializer>();
    }
}
