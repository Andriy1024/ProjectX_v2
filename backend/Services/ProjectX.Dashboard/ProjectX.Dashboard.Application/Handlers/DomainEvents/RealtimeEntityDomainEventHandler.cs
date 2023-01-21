using ProjectX.Core.Auth;
using ProjectX.Core.Realtime.Abstractions;
using ProjectX.Core.Realtime.Models;

namespace ProjectX.Dashboard.Application.Handlers.DomainEvents;

public class RealtimeEntityDomainEventHandler<TEntity, TEvent, TMessage> : IDomainEventHandler<TEvent>
    where TEvent : EntityDomainEvent<TEntity>
    where TMessage : IRealtimeMessage
{
    private readonly IRealtimeTransactionContext _realtime;
    private readonly IMapper _mapper;
    private readonly IUserContext _user;

    public RealtimeEntityDomainEventHandler(IRealtimeTransactionContext realtime, IMapper mapper, IUserContext user)
    {
        _realtime = realtime;
        _mapper = mapper;
        _user = user;
    }

    public Task Handle(TEvent notification, CancellationToken cancellationToken)
    {
        var messagePayload = GetMessage(notification);
        
        var receivers = GetReceivers(notification);

        var realTimeMessege = new RealtimeMessageContext(messagePayload);

        _realtime.Add(realTimeMessege, receivers);

        return Task.CompletedTask;
    }

    protected virtual TMessage GetMessage(TEvent notification) 
    {
        return _mapper.Map<TMessage>(notification.Entity);
    }

    protected virtual IEnumerable<int> GetReceivers(TEvent notification)
    {
        return new[] { _user.Id };
    }
}
