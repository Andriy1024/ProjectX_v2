using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace ProjectX.Core.Observability.Logging;

/// <summary>
/// https://github.com/osstotalsoft/nbb/tree/master/src/Correlation/NBB.Correlation.Serilog
/// </summary>
public class CorrelationLogEventEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        //TODO: Implement

        //logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("CorrelationId", CorrelationManager.GetCorrelationId()));
    }
}
