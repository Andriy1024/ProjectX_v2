using ProjectX.Core;
using ProjectX.FileStorage.Files.Models;
using System.Reflection;

namespace ProjectX.FileStorage.Files.Implementations;

public sealed class LocalFileStorage : IFileStorage
{
    public static readonly string RootLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "file_storage");

    public async Task<IFileAttributes> UploadAsync(UploadArgs options, CancellationToken cancellationToken = default)
    {
        options.ThrowIfNull();

        var physicalLocation = GetPhysicalPath(options.EntryInfo.Location);

        var physicalLocationWithName = Path.Combine(physicalLocation, options.EntryInfo.Name);

        if (!options.Override) 
        {
            ThrowIfFileExists(physicalLocationWithName);
        }

        if (Directory.Exists(physicalLocationWithName))
        {
            throw new Exception($"Cannot create file '{physicalLocationWithName}' because it already exists as a directory.");
        }

        CreateFolderIfNotExists(physicalLocation);

        var source = options.EntryStream;

        source.Seek(0, SeekOrigin.Begin);

        await using (var fileStream = new FileStream(physicalLocationWithName, FileMode.Create, FileAccess.Write))
        {
            await source.CopyToAsync(fileStream, cancellationToken);
        }

        return options.EntryInfo;
    }

    public async Task<Stream> DownloadAsync(DownloadArgs options, CancellationToken cancellationToken = default)
    {
        options.ThrowIfNull();

        var physicalLocation = GetPhysicalPath(options.Location);

        if (!File.Exists(physicalLocation))
        {
            throw new Exception($"File '{physicalLocation}' was not found.");
        }

        return new FileStream(physicalLocation, FileMode.Open, FileAccess.Read, FileShare.Read);
    }

    public Task DeleteAsync(DeleteArgs options, CancellationToken cancellationToken = default)
    {
        options.ThrowIfNull();

        var physicalLocation = GetPhysicalPath(options.Location);

        if (!File.Exists(physicalLocation))
        {
            throw new Exception("File not found.");
        }

        File.Delete(physicalLocation);

        return Task.CompletedTask;
    }

    private static string GetPhysicalPath(string path)
    {
        return Path.Combine(RootLocation, path);
    }

    private static void ThrowIfFileExists(string path)
    {
        if (File.Exists(path))
        {
            throw new Exception($"Cannot create file '{path}' because it already exists.");
        }
    }

    private static void CreateFolderIfNotExists(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
}