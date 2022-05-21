using ProjectX.Core;
using ProjectX.Tasks.Application.Contracts;

namespace ProjectX.Tasks.Application;

public class TasksQuery : IQuery<IEnumerable<TaskContarct>> 
{
}

public class CreateTaskCommand : ICommand<TaskContarct>
{
    public string Name { get; init; }

    public string Description { get; init; }
}

public class UpdateTaskCommand : ICommand<TaskContarct>
{
    public int Id { get; init; }

    public string Name { get; init; }

    public string Description { get; init; }
}

public class DeleteTaskCommand : ICommand 
{
    public int Id { get; init; }
}