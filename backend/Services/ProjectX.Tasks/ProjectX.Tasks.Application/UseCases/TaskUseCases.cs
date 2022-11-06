using FluentValidation;
using FluentValidation.Results;
using ProjectX.Core;
using ProjectX.Core.Validation;
using ProjectX.Tasks.Application.Contracts;

namespace ProjectX.Tasks.Application;

public class TasksQuery : IQuery<IEnumerable<TaskContarct>> 
{
}

public class CreateTaskCommand : ICommand<TaskContarct>, IValidatable
{
    public string Name { get; init; }

    public string Description { get; init; }

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

    public IEnumerable<ValidationFailure> Validate()
    {
        return this.Validate(command =>
        {
            command.RuleFor(x => x.Id).GreaterThan(0);
        });
    }
}