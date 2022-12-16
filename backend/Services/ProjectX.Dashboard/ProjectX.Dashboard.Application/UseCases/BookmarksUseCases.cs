using ProjectX.Dashboard.Application.Contracts;

namespace ProjectX.Dashboard.Application;

public class BookmarksQuery : IQuery<IEnumerable<BookmarkContarct>>
{
}

public class FindBookmarkQuery : IQuery<BookmarkContarct>
{
    public int Id { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Id).GreaterThan(0);
        });
    }
}

public class CreateBookmarkCommand : ICommand<BookmarkContarct>, IValidatable
{
    public string Name { get; init; }

    public string URL { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Name).NotEmpty();
            command.RuleFor(x => x.URL).NotEmpty();
        });
    }
}

public class UpdateBookmarkCommand : ICommand<BookmarkContarct>, IValidatable
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string URL { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Id).GreaterThan(0);
            command.RuleFor(x => x.Name).NotEmpty();
            command.RuleFor(x => x.URL).NotEmpty();
        });
    }
}

public class DeleteBookmarkCommand : ICommand, IValidatable
{
    public int Id { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Id).GreaterThan(0);
        });
    }
}