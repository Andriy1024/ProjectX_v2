using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ProjectX.Core;

namespace ProjectX.AspNetCore.Http;

/// <summary>
/// Represents Error handling middleware. Used to process error's user messages.
/// </summary>
public sealed class ErrorHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    public ErrorHandlerMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
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

            context.Response.StatusCode = 500;

            await context.Response.WriteAsJsonAsync(error);
        }
    }
}