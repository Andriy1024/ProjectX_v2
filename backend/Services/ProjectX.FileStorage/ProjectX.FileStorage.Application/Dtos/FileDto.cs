namespace ProjectX.FileStorage.Application;

public class FileDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Location { get; set; }

    public string Extension { get; set; }

    public string MimeType { get; set; }

    public long Size { get; set; }

    public DateTime CreatedAt { get; set; }
}