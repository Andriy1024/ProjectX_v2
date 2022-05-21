using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using ProjectX.Core;

namespace ProjectX.Tasks.API.SeedWork;

/// <summary>
/// Represents Error handling middleware. Used to process error's user messages.
/// </summary>
public sealed class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IActionResultExecutor<ObjectResult> _actionResultExecutor;

    public ErrorHandlerMiddleware(RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger,
        IActionResultExecutor<ObjectResult> actionResultExecutor)
    {
        _next = next;
        _logger = logger;
        _actionResultExecutor = actionResultExecutor;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Exception message: {ex.Message}.\nInner exception: {ex.InnerException?.Message}.\nStack trace: {ex.StackTrace}.");

            ResultOf<Unit> error = Error.From(ex);

            await SendResponseAsync(context, new InternalServerErrorObjectResult(error));
        }
    }

    /// <summary>
    /// Executes passed action result.
    /// </summary>
    /// <param name="context">HttpContext of current request.</param>
    /// <param name="objectResult">Instance of ObjectResult implementation, contains error data.</param>
    private Task SendResponseAsync(HttpContext context, ObjectResult objectResult) =>
            _actionResultExecutor.ExecuteAsync(new ActionContext() { HttpContext = context }, objectResult);
}