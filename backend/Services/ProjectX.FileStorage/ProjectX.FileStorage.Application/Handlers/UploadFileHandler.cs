using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using ProjectX.Core;
using ProjectX.Core.Validation;
using ProjectX.FileStorage.Application;
using ProjectX.FileStorage.Database.Abstractions;
using ProjectX.FileStorage.Files;
using ProjectX.FileStorage.Files.Models;
using ProjectX.FileStorage.Persistence.Database.Documents;

namespace ProjectX.FileStorage.Infrastructure.Handlers;

public sealed class UploadFileCommand : ICommand<FileDto>, IValidatable
{
    public required IFormFile File { get; init; }

    public required string Location { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(x => 
        {
            x.RuleFor(a => a.File).NotNull();
            x.RuleFor(a => a.Location).NotEmpty();
        });
    }
}

public sealed class UploadFileHandler : ICommandHandler<UploadFileCommand, FileDto>
{
    private readonly IFileStorage _fileStorage;
    private readonly IMongoRepository<FileDocument, Guid> _repository;
    private readonly IMapper _mapper;

    public UploadFileHandler(IFileStorage fileStorage, IMongoRepository<FileDocument, Guid> repository, IMapper mapper)
    {
        _fileStorage = fileStorage;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultOf<FileDto>> Handle(UploadFileCommand command, CancellationToken cancellationToken)
    {
        var uploadOptions = UploadArgs.Create(command.File, command.Location, @override: false);
       
        var storageEntry = await _fileStorage.UploadAsync(uploadOptions, cancellationToken);

        var fileDocument = new FileDocument 
        {
            Id = Guid.NewGuid(),
            CreatedAt= DateTime.UtcNow,
            Name = storageEntry.Name,
            Extension= storageEntry.Extension,
            MimeType= storageEntry.MimeType,
            Location= storageEntry.Location,
            Size = storageEntry.Size
        }; 

        await _repository.AddAsync(fileDocument, cancellationToken);

        return _mapper.Map<FileDto>(fileDocument);
    }
}