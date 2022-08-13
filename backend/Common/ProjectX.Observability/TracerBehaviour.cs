using MediatR;

namespace ProjectX.Observability;

public class TracerBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ITracer _tracer;

    public TracerBehaviour(ITracer tracer)
    {
        _tracer = tracer;
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
    {
        return await _tracer.Trace<TRequest, TResponse>(() => next());
    }
}