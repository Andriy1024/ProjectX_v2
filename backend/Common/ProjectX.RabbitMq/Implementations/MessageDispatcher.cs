using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProjectX.Core.Context;
using ProjectX.RabbitMq.Pipeline;
using ProjectX.RabbitMq.Tracing;

namespace ProjectX.RabbitMq.Implementations;

internal class MessageDispatcher : IMessageDispatcher
{
    private readonly IServiceScopeFactory _scopeFactory;


    public MessageDispatcher(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task HandleAsync(SubscriberRequest input)
    {
        await using (var scope = _scopeFactory.CreateAsyncScope()) 
        {
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var contextAccessor = scope.ServiceProvider.GetRequiredService<IContextAccessor>();

            var context = input.RabbitPrperties.BasicProperties.ExtractContext();

            contextAccessor.Context = context;

            await mediator.Send(input.IntegrationEvent);
        }
    }
}