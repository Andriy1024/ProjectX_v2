using ProjectX.Dashboard.Application.Contracts;

namespace ProjectX.Dashboard.Application.Handlers;

public sealed class NotesHandlers :
    IQueryHandler<NotesQuery, IEnumerable<NoteContarct>>,
    IQueryHandler<FindNoteQuery, NoteContarct>,
    ICommandHandler<CreateNoteCommand, NoteContarct>,
    ICommandHandler<UpdateNoteCommand, NoteContarct>,
    ICommandHandler<DeleteNoteCommand>
{
    private readonly IRepository<NoteEntity> _repository;
    private readonly IMapper _mapper;

    public NotesHandlers(IRepository<NoteEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultOf<IEnumerable<NoteContarct>>> Handle(NotesQuery query, CancellationToken cancellationToken)
    {
        var notes = await _repository.GetAsync(cancellationToken: cancellationToken);

        return _mapper.Map<NoteContarct[]>(notes);
    }

    public async Task<ResultOf<NoteContarct>> Handle(FindNoteQuery query, CancellationToken cancellationToken)
    {
        var maybeNote = await _repository.FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (maybeNote.IsFailed)
        {
            return maybeNote.Error!;
        }

        return _mapper.Map<NoteContarct>(maybeNote.Data);
    }

    public async Task<ResultOf<NoteContarct>> Handle(CreateNoteCommand command, CancellationToken cancellationToken)
    {
        var note = NoteEntity.Create(command.Title, command.Content);

        await _repository.InsertAsync(note, cancellationToken);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return _mapper.Map<NoteContarct>(note);
    }

    public async Task<ResultOf<NoteContarct>> Handle(UpdateNoteCommand command, CancellationToken cancellationToken)
    {
        var maybeNote = await _repository.FirstOrDefaultAsync(t => t.Id == command.Id);

        if (maybeNote.IsFailed)
        {
            return maybeNote.Error!;
        }

        maybeNote.Data!.Update(command.Title, command.Content);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return _mapper.Map<NoteContarct>(maybeNote.Data);
    }

    public async Task<ResultOf<Unit>> Handle(DeleteNoteCommand command, CancellationToken cancellationToken)
    {
        var maybeNote = await _repository.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (maybeNote.IsFailed)
        {
            return maybeNote.Error!;
        }

        maybeNote.Data!.Remove();

        _repository.Remove(maybeNote.Data);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return ResultOf<Unit>.Unit;
    }
}
