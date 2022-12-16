using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectX.AspNetCore.Http;
using ProjectX.Dashboard.Application;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectX.Dashboard.API.Controllers;

[Route("api/tasks")]
//[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class TasksController : ProjectXController
{
    [HttpGet]
    public Task<IActionResult> GetTasks([FromQuery] TasksQuery query, CancellationToken cancellationToken)
        => Send(query, cancellationToken);

    [HttpPost]
    public Task<IActionResult> CreateTask([FromBody] CreateTaskCommand command)
        => Send(command);

    [HttpPut]
    public Task<IActionResult> UpdateTask([FromBody] UpdateTaskCommand command)
        => Send(command);

    [HttpDelete]
    public Task<IActionResult> DeleteTask([FromBody] DeleteTaskCommand command)
        => Send(command);
}