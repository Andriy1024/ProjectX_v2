using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace ProjectX.Core.Context;

internal sealed class ContextProvider : IContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IContextAccessor _contextAccessor;

    public ContextProvider(IHttpContextAccessor httpContextAccessor, IContextAccessor contextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _contextAccessor = contextAccessor;
    }

    public IContext Current()
    {
        if (_contextAccessor.Context is not null)
        {
            return _contextAccessor.Context;
        }

        IContext context;
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is not null)
        {
            var traceId = httpContext.TraceIdentifier;
            var correlationId = httpContext.GetCorrelationId() ?? Guid.NewGuid().ToString("N");
            var userId = httpContext.User.Identity?.Name;
            
            context = new Context(
                activityId: Activity.Current?.Id ?? ActivityTraceId.CreateRandom().ToString(),
                traceId: traceId,
                correlationId: correlationId,
                messageId: string.Empty, 
                //causationId: string.Empty, 
                userId: userId);
        }
        else
        {
            context = new Context(
                activityId: Activity.Current?.Id ?? ActivityTraceId.CreateRandom().ToString(),
                traceId: string.Empty,
                correlationId: Guid.NewGuid().ToString("N"));
        }

        _contextAccessor.Context = context;

        return context;
    }
}