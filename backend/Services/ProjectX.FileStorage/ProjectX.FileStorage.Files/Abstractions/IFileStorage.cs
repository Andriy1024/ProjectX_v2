using ProjectX.FileStorage.Files.Models;

namespace ProjectX.FileStorage.Files;

public interface IFileStorage
{
    Task<IFileAttributes> UploadAsync(UploadArgs options, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(DownloadArgs options, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteArgs options, CancellationToken cancellationToken = default);
}