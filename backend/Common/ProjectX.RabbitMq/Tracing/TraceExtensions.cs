using ProjectX.Core.Context;
using ProjectX.RabbitMq.Publisher;
using System.Text;

namespace ProjectX.RabbitMq.Tracing;

public static class TraceExtensions
{
    public static void Enrich(
        this IBasicProperties props,
        PublisherContext publisher,
        IContext context)
    {
        var activity = System.Diagnostics.Activity.Current;

        var activityId = activity?.Id ?? context.ActivityId;

        props.Headers ??= new Dictionary<string, object>();
        props.Headers.Add(TracingConstants.ActivityId, activityId);
        props.Headers.Add(TracingConstants.TraceId, context.TraceId);
        props.Headers.Add(TracingConstants.CorrelationId, context.CorrelationId);

        if (activity != null)
        {
            // These tags are added demonstrating the semantic conventions of the OpenTelemetry messaging specification
            // See:
            //   * https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/messaging.md#messaging-attributes
            //   * https://github.com/open-telemetry/opentelemetry-specification/blob/main/specification/trace/semantic_conventions/messaging.md#rabbitmq
            activity?.SetTag(TracingConstants.SystemKey, TracingConstants.SystemValue);
            activity?.SetTag(TracingConstants.DestinationKind, TracingConstants.DestinationQueue);
            activity?.SetTag(TracingConstants.DestinationKey, publisher.Exchange.Name);
            activity?.SetTag(TracingConstants.RoutingKey, publisher.RoutingKey);
            //activity?.SetTag("messaging.rabbitmq.queue", "queue_name");
        }
    }

    public static Context ExtractContext(this IBasicProperties prop)
    {
        var activityId = GetHeaderValue(prop, TracingConstants.ActivityId);
        var traceId = GetHeaderValue(prop, TracingConstants.TraceId);
        var correlationId = GetHeaderValue(prop, TracingConstants.CorrelationId);

        return new Context(
            activityId: activityId,
            traceId: traceId,
            correlationId: correlationId);
    }

    private static string GetHeaderValue(IBasicProperties properties, string key)
      => properties.Headers.TryGetValue(key, out var bytes)
          ? Encoding.UTF8.GetString(bytes as byte[] ?? Array.Empty<byte>())
          : string.Empty;
}
