using ProjectX.Dashboard.Application.Contracts;

namespace ProjectX.Dashboard.Application;

public class NotesQuery : IQuery<IEnumerable<NoteContarct>>
{
}

public class FindNoteQuery : IQuery<NoteContarct>, IValidatable
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

public class CreateNoteCommand : ICommand<NoteContarct>, IValidatable
{
    public string Title { get; init; }

    public string Content { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Title).NotEmpty();
        });
    }
}

public class UpdateNoteCommand : ICommand<NoteContarct>, IValidatable
{
    public int Id { get; init; }

    public string Title { get; init; }

    public string Content { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Id).GreaterThan(0);
            command.RuleFor(x => x.Title).NotEmpty();
        });
    }
}

public class DeleteNoteCommand : ICommand, IValidatable
{
    public int Id { get; }

    public DeleteNoteCommand(int id)
        => Id = id;

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Id).GreaterThan(0);
        });
    }
}