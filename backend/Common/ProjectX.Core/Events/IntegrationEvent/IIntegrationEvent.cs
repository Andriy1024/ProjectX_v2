using ProjectX.Core.Abstractions;

namespace ProjectX.Core.Events.IntegrationEvent;

public interface IIntegrationEvent : IRequest, IHasTransaction
{
    public Guid Id { get; set; }
}