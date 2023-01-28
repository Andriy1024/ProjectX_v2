namespace ProjectX.FileStorage.Persistence.FileStorage.Models;

public record DownloadOptions(string Location)
{
    public DownloadOptions(string location, string name)
        : this(Path.Combine(location, name))
    {
    }
}
