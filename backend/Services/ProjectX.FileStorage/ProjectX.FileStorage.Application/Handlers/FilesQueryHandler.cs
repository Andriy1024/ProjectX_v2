using AutoMapper;
using MongoDB.Driver;
using ProjectX.Core;
using ProjectX.FileStorage.Application;
using ProjectX.FileStorage.Database.Abstractions;
using ProjectX.FileStorage.Persistence.Database.Documents;

namespace ProjectX.FileStorage.Infrastructure.Handlers;

public sealed class FilesQuery : IQuery<FileDto[]> {}

public sealed class FilesQueryHandler : IQueryHandler<FilesQuery, FileDto[]>
{
    private readonly IMongoRepository<FileDocument, Guid> _repository;
    private readonly IMapper _mapper;

    public FilesQueryHandler(IMongoRepository<FileDocument, Guid> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultOf<FileDto[]>> Handle(FilesQuery query, CancellationToken cancellationToken)
    {
        var files = await _repository.Collection.AsQueryable().ToListAsync(cancellationToken);

        return _mapper.Map<FileDto[]>(files);
    }
}