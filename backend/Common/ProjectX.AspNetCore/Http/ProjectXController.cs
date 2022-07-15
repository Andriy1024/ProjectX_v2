using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Core;

namespace ProjectX.AspNetCore.Http;

/// <summary>
/// Represents base api controller with integrated IMediator service, and response mapping.
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class ProjectXController : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

    protected async Task<IActionResult> Send<TResult>(ICommand<TResult> command)
        => MapResponse(await Mediator.Send(command));

    protected async Task<IActionResult> Send<TResult>(IQuery<TResult> command, CancellationToken ct)
        => MapResponse(await Mediator.Send(command, ct));

    protected IActionResult MapResponse<T>(ResultOf<T> response)
        => response.ThrowIfNull().IsFailed ? MapError(response) : Ok(response);

    private static IActionResult MapError<T>(ResultOf<T> response)
        => response.Error.ThrowIfNull().Type switch
        {
            ErrorType.ServerError => new InternalServerErrorObjectResult(response),
            ErrorType.NotFound => new NotFoundObjectResult(response),
            ErrorType.InvalidData => new BadRequestObjectResult(response),
            ErrorType.InvalidPermission => new ForbiddenObjectResult(response),
            _ => throw new ArgumentOutOfRangeException($"Invalid error type: {response.Error}")
        };
}