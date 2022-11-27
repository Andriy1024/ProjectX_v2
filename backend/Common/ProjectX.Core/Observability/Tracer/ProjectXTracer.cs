using ProjectX.Core;
using ProjectX.Core.Context;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace ProjectX.Core.Observability;

internal class ProjectXTracer : ITracer
{
    public static readonly string Name = "ProjectX";

    private static readonly ActivitySource Source = new ActivitySource(Name);

    private readonly IContextProvider _contextProvider;

    public ProjectXTracer(IContextProvider contextProvider)
    {
        _contextProvider = contextProvider;
    }

    public void Trace([NotNull] TraceCode code, [NotNull] string description)
    {
        var activity = Activity.Current;

        if (activity == null) return;

        activity.AddTag("otel.status_code", code.Code);

        activity.AddTag("otel.status_description", description);
    }

    public void Trace([NotNull] Exception error)
    {
        Trace(TraceCode.Error, error.Message);
    }

    public Task<TResult> Trace<TActionType, TResult>(Func<Task<TResult>> func)
        where TResult : notnull
    {
        return Trace(typeof(TActionType).Name, func);
    }

    public async Task<TResult> Trace<TResult>(string actionName, Func<Task<TResult>> func)
        where TResult : notnull
    {
        var context = _contextProvider.Current();

        using (Activity? activity = Source.StartActivity(actionName, ActivityKind.Producer, parentId: context.ActivityId))
        {
            try
            {
                activity?.SetTag("correlation_id", context.CorrelationId);
                activity?.SetTag("causation_id", context.CausationId);

                var result = await func();

                var code = result is IResult r && r.IsFailed
                                  ? TraceCode.Error
                                  : TraceCode.Success;

                Trace(code, result.ToString()!);

                return result;
            }
            catch (Exception ex)
            {
                Trace(ex);

                throw;
            }
        }
    }

    public Task Trace<TActionType>(Func<Task> func)
    {
        return Trace(typeof(TActionType).Name, func);
    }

    public Task Trace(string actionName, Func<Task> func)
    {
        return Trace(actionName, async () => 
        {
            await func();

            return Unit.Value;
        });
    }
}