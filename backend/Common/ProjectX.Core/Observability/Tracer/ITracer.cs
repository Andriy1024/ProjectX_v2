namespace ProjectX.Core.Observability;

public interface ITracer
{
    void Trace(TraceCode code, string description);

    void Trace(Exception error);

    Task<TResult> Trace<TActionType, TResult>(Func<Task<TResult>> func)
        where TResult : notnull;

    Task<TResult> Trace<TResult>(string actionName, Func<Task<TResult>> func)
        where TResult : notnull;

    Task Trace<TActionType>(Func<Task> func);

    Task Trace(string actionName, Func<Task> func);
}