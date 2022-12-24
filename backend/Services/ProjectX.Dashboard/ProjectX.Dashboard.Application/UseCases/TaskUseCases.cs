using ProjectX.Dashboard.Application.Contracts;

namespace ProjectX.Dashboard.Application;

public class TasksQuery : IQuery<IEnumerable<TaskContarct>> 
{
}

public class FindTaskQuery : IQuery<TaskContarct>
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

public class CreateTaskCommand : ICommand<TaskContarct>, IValidatable
{
    public string Name { get; init; }

    public string? Description { get; init; }

    public bool Completed { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Name).NotEmpty();
        });
    }
}

public class UpdateTaskCommand : ICommand<TaskContarct>, IValidatable
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }

    public bool Completed { get; init; }

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Id).GreaterThan(0);
            command.RuleFor(x => x.Name).NotEmpty();
        });
    }
}

public class DeleteTaskCommand : ICommand, IValidatable
{
    public int Id { get; init; }

    public DeleteTaskCommand(int id)
        => Id = id;
    
    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Id).GreaterThan(0);
        });
    }
}