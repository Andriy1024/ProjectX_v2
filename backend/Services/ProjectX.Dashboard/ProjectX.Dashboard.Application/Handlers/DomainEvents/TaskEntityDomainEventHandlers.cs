using ProjectX.Core.Realtime.Abstractions;
using ProjectX.Core.Realtime.Models;
using ProjectX.Realtime.Messages;

namespace ProjectX.Dashboard.Application.Handlers.DomainEvents;

public class TaskEntityDomainEventHandlers
    : IDomainEventHandler<EntityCreated<TaskEntity>>
{
    private readonly IRealtimeTransactionContext _realtime;

    public TaskEntityDomainEventHandlers(IRealtimeTransactionContext realtime)
    {
        _realtime = realtime;
    }

    public async Task Handle(EntityCreated<TaskEntity> domainEvent, CancellationToken cancellationToken)
    {
        var realTimeMessege = new RealtimeMessageContext(new TaskCreatedMessage 
        {
            Id = domainEvent.Entity.Id,
            Name = domainEvent.Entity.Name,
            Description = domainEvent.Entity.Description,
            Completed = domainEvent.Entity.Completed,
            CreatedAt = domainEvent.Entity.CreatedAt,
            UpdatedAt = domainEvent.Entity.UpdatedAt
        });

        _realtime.Add(realTimeMessege, new[] { 1 });
    }
}
