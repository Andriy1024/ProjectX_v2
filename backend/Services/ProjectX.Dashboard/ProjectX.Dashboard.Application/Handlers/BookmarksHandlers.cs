using ProjectX.Dashboard.Application.Contracts;

namespace ProjectX.Dashboard.Application.Handlers;

public sealed class BookmarksHandlers :
    IQueryHandler<BookmarksQuery, IEnumerable<BookmarkContarct>>,
    IQueryHandler<FindBookmarkQuery, BookmarkContarct>,
    ICommandHandler<CreateBookmarkCommand, BookmarkContarct>,
    ICommandHandler<UpdateBookmarkCommand, BookmarkContarct>,
    ICommandHandler<DeleteBookmarkCommand>
{
    private readonly IRepository<BookmarkEntity> _repository;
    private readonly IMapper _mapper;

    public BookmarksHandlers(IRepository<BookmarkEntity> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResultOf<IEnumerable<BookmarkContarct>>> Handle(BookmarksQuery query, CancellationToken cancellationToken)
    {
        var notes = await _repository.GetAsync(cancellationToken: cancellationToken);

        return _mapper.Map<BookmarkContarct[]>(notes);
    }

    public async Task<ResultOf<BookmarkContarct>> Handle(FindBookmarkQuery request, CancellationToken cancellationToken)
    {
        var maybeBookmark = await _repository.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (maybeBookmark.IsFailed)
        {
            return maybeBookmark.Error!;
        }

        return _mapper.Map<BookmarkContarct>(maybeBookmark.Data);
    }

    public async Task<ResultOf<BookmarkContarct>> Handle(CreateBookmarkCommand command, CancellationToken cancellationToken)
    {
        var bookmark = BookmarkEntity.Create(command.Name, command.URL);

        await _repository.InsertAsync(bookmark, cancellationToken);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return _mapper.Map<BookmarkContarct>(bookmark);
    }

    public async Task<ResultOf<BookmarkContarct>> Handle(UpdateBookmarkCommand command, CancellationToken cancellationToken)
    {
        var maybeBookmark = await _repository.FirstOrDefaultAsync(t => t.Id == command.Id);

        if (maybeBookmark.IsFailed)
        {
            return maybeBookmark.Error!;
        }

        maybeBookmark.Data!.Update(command.Name, command.URL);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return _mapper.Map<BookmarkContarct>(maybeBookmark.Data);
    }

    public async Task<ResultOf<Unit>> Handle(DeleteBookmarkCommand command, CancellationToken cancellationToken)
    {
        var maybeBookmark = await _repository.FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken);

        if (maybeBookmark.IsFailed)
        {
            return maybeBookmark.Error!;
        }

        maybeBookmark.Data!.Remove();

        _repository.Remove(maybeBookmark.Data);

        await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

        return ResultOf<Unit>.Unit;
    }
}
