using ProjectX.FileStorage.Domain;
using ProjectX.FileStorage.Persistence.FileStorage.Models;

namespace ProjectX.FileStorage.Application.Interfaces;

public interface IFileStorage
{
    Task<IFileAttributes> UploadAsync(UploadOptions options, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(DownloadOptions options, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteOptions options, CancellationToken cancellationToken = default);
}