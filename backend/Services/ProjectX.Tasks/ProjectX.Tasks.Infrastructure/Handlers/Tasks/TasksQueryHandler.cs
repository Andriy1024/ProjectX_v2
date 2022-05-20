using ProjectX.Persistence;
using ProjectX.Tasks.Application.Queries.Tasks;
using ProjectX.Tasks.Domain.Entities;

namespace ProjectX.Tasks.Infrastructure.Handlers.Tasks;

public sealed class TasksQueryHandler : QueryHandler<TasksQuery, IEnumerable<TasksQuery.Result>>
{
    private readonly IRepository<TaskEntity> _repository;

    public TasksQueryHandler(IRepository<TaskEntity> repository)
    {
        _repository = repository;
    }

    public override async Task<Response<IEnumerable<TasksQuery.Result>>> Handle(TasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _repository.GetAsync(cancellationToken: cancellationToken);

            return tasks.Select(t => new TasksQuery.Result
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description
            }).ToArray();
        }
        catch (Exception e)
        {
            return Failed(Error.From(e));
        }
    }
}
