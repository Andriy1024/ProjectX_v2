using Microsoft.AspNetCore.Http;
using ProjectX.Core;

namespace ProjectX.FileStorage.Files.Models;

public record UploadArgs(
    Stream EntryStream,
    IFileAttributes EntryInfo,
    //Override file if it exists when true, either throw exception.
    bool Override)
{
    public static UploadArgs Create(IFormFile file, string location, bool @override) 
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

        return new UploadArgs(file.OpenReadStream(), EntryInfo, @override);
    }
}
