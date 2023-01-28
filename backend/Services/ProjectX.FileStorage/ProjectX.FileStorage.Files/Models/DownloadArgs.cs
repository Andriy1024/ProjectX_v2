namespace ProjectX.FileStorage.Files.Models;

public record DownloadArgs(string Location)
{
    public DownloadArgs(string location, string name)
        : this(Path.Combine(location, name))
    {
    }
}
