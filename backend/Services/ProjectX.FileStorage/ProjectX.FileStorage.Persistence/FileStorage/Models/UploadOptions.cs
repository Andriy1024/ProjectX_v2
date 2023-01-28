using Microsoft.AspNetCore.Http;
using ProjectX.Core;
using ProjectX.FileStorage.Domain;

namespace ProjectX.FileStorage.Persistence.FileStorage.Models;

public record UploadOptions(
    Stream EntryStream,
    IFileAttributes EntryInfo,
    //Override file if it exists when true, either throw exception.
    bool Override)
{
    public static UploadOptions Create(IFormFile file, string location, bool @override = false) 
    {
        var size = file.ThrowIfNull().Length;
        var extension = FileUtill.TryGetExtension(file.FileName);
        var mimeType = FileUtill.TryGetContentType(file.FileName);
        var name = FileUtill.GenerateFileName(extension);
        var EntryInfo = new FileAttributes()
        {
            Name = name,
            Location = location,
            Extension = extension,
            MimeType = mimeType,
            Size = size,
        };

        return new UploadOptions(file.OpenReadStream(), EntryInfo, @override);
    }
}
