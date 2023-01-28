using Microsoft.Extensions.DependencyInjection;
using ProjectX.FileStorage.Files;
using ProjectX.FileStorage.Files.Implementations;

namespace ProjectX.FileStorage.Persistence.FileStorage;

public static class FileStorageSetup
{
    public static IServiceCollection AddFileStorage(this IServiceCollection services)
        => services.AddSingleton<IFileStorage, LocalFileStorage>();
}