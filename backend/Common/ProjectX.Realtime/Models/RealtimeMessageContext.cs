namespace ProjectX.Core.Realtime.Models;

public class RealtimeMessageContext
{
    public string Type { get; set; }

    public object Message { get; set; }

    public RealtimeMessageContext()
    {
    }

    public RealtimeMessageContext(IRealtimeMessage message)
    {
        Message = message.ThrowIfNull();
        Type = Message.GetType().Name;
    }
}