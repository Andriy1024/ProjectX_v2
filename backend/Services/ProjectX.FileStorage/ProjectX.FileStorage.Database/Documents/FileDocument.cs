using MongoDB.Bson.Serialization.Attributes;
using ProjectX.FileStorage.Database.Abstractions;

namespace ProjectX.FileStorage.Persistence.Database.Documents;

public class FileDocument : IDocumentEntry<Guid> 
{
    public static string Collection => "Files";

    [BsonId]
    public required Guid Id { get; init; }

    [BsonElement("name")]
    public required string Name { get; init; }

    public required string Location { get; init; }

    public required string Extension { get; init; }

    public required string MimeType { get; init; }

    public required long Size { get; init; }

    public required DateTime CreatedAt { get; init; }
}