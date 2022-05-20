using Microsoft.AspNetCore.Mvc;
using ProjectX.Tasks.API.SeedWork;
using ProjectX.Tasks.Application.Queries.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectX.Tasks.API.Controllers;

[Route("api/tasks")]
public class TasksController : ProjectXController
{
    [HttpGet]
    public Task<IActionResult> GetTasksAsync(CancellationToken cancellationToken)
        => Send(new TasksQuery(), cancellationToken);
}
