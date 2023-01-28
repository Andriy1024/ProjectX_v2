using ProjectX.Core;
using ProjectX.FileStorage.Database.Abstractions;
using ProjectX.FileStorage.Files;
using ProjectX.FileStorage.Persistence.Database.Documents;

namespace ProjectX.FileStorage.Infrastructure.Handlers;

public sealed class DownloadFileQuery : IQuery<DownloadFileQuery.Response>
{
    public required Guid Id { get; init; }

    public sealed class Response
    {
        public required Stream File { get; init; }

        public required string MimeType { get; init; }
    }
}

public sealed class DownloadFileHandler : IQueryHandler<DownloadFileQuery, DownloadFileQuery.Response>
{
    private readonly IFileStorage _fileStorage;
    private readonly IMongoRepository<FileDocument, Guid> _repository;

    public DownloadFileHandler(IFileStorage fileStorage, IMongoRepository<FileDocument, Guid> repository)
    {
        _fileStorage = fileStorage;
        _repository = repository;
    }

    public async Task<ResultOf<DownloadFileQuery.Response>> Handle(DownloadFileQuery query, CancellationToken cancellationToken)
    {
        var fileDocument = await _repository.GetAsync(query.Id, cancellationToken);

        if (fileDocument == null)
        {
            return ApplicationError.NotFound();
        }

        var file = await _fileStorage.DownloadAsync(new(fileDocument.Location, fileDocument.Name), cancellationToken);

        return new DownloadFileQuery.Response() 
        {
            File = file,
            MimeType = fileDocument.MimeType
        };
    }
}