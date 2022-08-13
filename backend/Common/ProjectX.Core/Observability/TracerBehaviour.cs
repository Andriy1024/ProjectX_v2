namespace ProjectX.Core.Observability;

public class TracerBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : notnull
{
    private readonly ITracer _tracer;

    public TracerBehaviour(ITracer tracer)
    {
        _tracer = tracer;
    }

    public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        return _tracer.Trace<TRequest, TResponse>(() => next());
    }
}