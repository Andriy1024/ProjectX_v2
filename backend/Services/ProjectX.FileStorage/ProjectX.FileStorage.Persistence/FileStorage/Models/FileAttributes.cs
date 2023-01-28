using ProjectX.FileStorage.Domain;

namespace ProjectX.FileStorage.Persistence.FileStorage.Models;

public record FileAttributes : IFileAttributes
{
    public required string Name { get; init; }

    public required string Location { get; init; }

    public required string Extension { get; init; }

    public required string MimeType { get; init; }

    public required long Size { get; init; }
}