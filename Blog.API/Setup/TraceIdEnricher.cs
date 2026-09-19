using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Blog.API.Setup;

// Makes the request's/message's trace ID visible in every log line —
// System.Diagnostics.Activity already propagates this automatically
// across the HTTP hop (blog-gateway -> Blog.API) and the MassTransit hop
// (Blog.API -> RabbitMQ -> blog-notifications); this just surfaces it.
// Named "TraceId", not "RequestId" (which RemoveExtraPropertiesEnricher
// strips) — RequestId is ASP.NET Core's own per-request id and doesn't
// propagate across services; TraceId is the W3C Trace Context id that
// does (learning-notes/notes/49-distributed-tracing-correlation-ids.md).
internal class TraceIdEnricher : ILogEventEnricher
{
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var traceId = Activity.Current?.TraceId.ToString();
        if (!string.IsNullOrEmpty(traceId))
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("TraceId", traceId));
        }
    }
}
