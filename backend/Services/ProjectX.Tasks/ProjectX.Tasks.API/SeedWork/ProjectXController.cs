using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProjectX.Core.Response;
using ProjectX.Core.CQRS;
using ProjectX.Core.Errors;
using ProjectX.Core.Extensions;

namespace ProjectX.Tasks.API.SeedWork;

/// <summary>
/// Represents base api controller with integrated IMediator service, and response mapping.
/// </summary>
[ApiController]
[Produces("application/json")]
public abstract class ProjectXController : ControllerBase
{
    protected IMediator Mediator => HttpContext.RequestServices.GetRequiredService<IMediator>();

    //protected async Task<IActionResult> Send(ICommand command)
    //    => MapResponse(await Mediator.Send(command));

    protected async Task<IActionResult> Send<TResult>(ICommand<TResult> command)
        => MapResponse(await Mediator.Send(command));

    protected async Task<IActionResult> Send<TResult>(IQuery<TResult> command, CancellationToken ct)
        => MapResponse(await Mediator.Send(command, ct));

    protected IActionResult MapResponse<T>(Response<T> response)
        => response.ThrowIfNull().IsSuccess ? Ok(response) : MapError(response);
   
    protected IActionResult MapResponse<T>(T response)
        => Ok(ResponseFactory.Success(response.ThrowIfNull()));
    
    private static IActionResult MapError<T>(Response<T> response)
        => response.Error.ThrowIfNull().Type switch 
        {
            ErrorType.ServerError => new InternalServerErrorObjectResult(response),
            ErrorType.NotFound => new NotFoundObjectResult(response),
            ErrorType.InvalidData => new BadRequestObjectResult(response),
            ErrorType.InvalidPermission => new ForbiddenObjectResult(response),
            _ => throw new ArgumentOutOfRangeException($"Invalid error type: {response.Error}")
        };
}