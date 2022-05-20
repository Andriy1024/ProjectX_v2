using Microsoft.EntityFrameworkCore;
using ProjectX.Tasks.Application.Queries.Tasks;
using ProjectX.Tasks.Persistence.Context;

namespace ProjectX.Tasks.Infrastructure.Handlers.Tasks;

public sealed class TasksQueryHandler : QueryHandler<TasksQuery, IEnumerable<TasksQuery.Result>>
{
    private readonly TasksDbContext _dbContext;

    public TasksQueryHandler(TasksDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override async Task<Response<IEnumerable<TasksQuery.Result>>> Handle(TasksQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var tasks = await _dbContext.Tasks.Select(t => new TasksQuery.Result
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description
            }).ToArrayAsync();

            return tasks;
        }
        catch (Exception e)
        {
            return Failed(Error.From(e));
        }
    }
}
