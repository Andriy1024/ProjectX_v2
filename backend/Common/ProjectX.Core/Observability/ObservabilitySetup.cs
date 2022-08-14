using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Enrichers.Span;
using System.Diagnostics;
using System.Reflection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Serilog.Events;

namespace ProjectX.Core.Observability;

public static class ObservabilitySetup
{
    public static IServiceCollection AddTracer(this IServiceCollection services)
        => services.AddSingleton<ITracer, ProjectXTracer>();

    public static IServiceCollection AddTracerBehaviour(this IServiceCollection services)
        => services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracerBehaviour<,>));

    public static IServiceCollection AddObservabilityServices(this WebApplicationBuilder app) 
        => app
        .AddLogging()
        .AddOpenTelemetry()
        .Services
        .AddTracer()
        .AddTracerBehaviour();

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder app)
    {
        app.Services.AddLogging(builder =>
        {
            var loggerConfig = new LoggerConfiguration()
                 .Enrich.WithProperty("Application", "TEST APP NAME")
                 .MinimumLevel.Information()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                 .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                 .Enrich.FromLogContext()
                 .Enrich.WithSpan()
                 .WriteTo.Console()
                 .WriteTo.Seq(EnvironmentVariables.SEQ_URI)
                 .WriteTo.File(
                    path: "logs/log.txt", 
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    //retainedFileCountLimit: max number of log files
                    retainedFileCountLimit: 30)
                 .ReadFrom.Configuration(app.Configuration);

            Log.Logger = loggerConfig.CreateLogger();

            builder.ClearProviders()
                   .AddSerilog();
        });

        return app;
    }

    public static WebApplicationBuilder AddOpenTelemetry(this WebApplicationBuilder app)
    {
        Activity.DefaultIdFormat = ActivityIdFormat.W3C;

        string serviceVersion = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? string.Empty;

        var serviceName = $"{app.Environment.ApplicationName}.{app.Environment.EnvironmentName}";

        app.Services
            .AddOpenTelemetryTracing((builder) => builder
            .AddSource(ProjectXTracer.Name)
            .AddJaegerExporter(o => 
            {
                //ENV: OTEL_EXPORTER_JAEGER_ENDPOINT
                o.Endpoint = new Uri(EnvironmentVariables.JAEGER_URI);
            })
            .AddConsoleExporter(options => 
            {
                options.Targets = ConsoleExporterOutputTargets.Console;
            })
            .AddAspNetCoreInstrumentation(options =>
            {
                options.RecordException = true;
                options.Enrich = (activity, @event, @object) =>
                {
                    if (@event == "OnStopActivity")
                    {
                        ExtractContextFromResponse(activity, @object);
                    }
                };
            })
            .AddHttpClientInstrumentation(options =>
            {
                options.Filter = message =>
                    message != null &&
                    message.RequestUri != null &&
                    !message.RequestUri.Host.Contains("visualstudio");
            })
            .AddSqlClientInstrumentation(options =>
            {
                options.EnableConnectionLevelAttributes = true;
                options.SetDbStatementForStoredProcedure = true;
                options.SetDbStatementForText = true;
                options.RecordException = true;
            })
            .SetSampler(new AlwaysOnSampler())
            .SetResourceBuilder(ResourceBuilder
                .CreateDefault()
                .AddService(serviceName, serviceVersion: serviceVersion, serviceInstanceId: Environment.MachineName)));

        return app;

        static void ExtractContextFromResponse(Activity activity, object @object)
        {
            if (activity == null) return;

            var httpContext = (@object as HttpResponse)?.HttpContext;

            if (httpContext?.User == null) return;

            var identityId = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimType.IdentityId)?.Value;

            if (!string.IsNullOrWhiteSpace(identityId))
            {
                activity.SetTag("identity.id", identityId);
            }

            var identityRole = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimType.IdentityRole)?.Value;

            if (!string.IsNullOrWhiteSpace(identityRole))
            {
                activity.SetTag("identity.role", identityRole);
            }

            //httpContext.Response.Headers.Add("trace-id", activity.TraceId.ToString() ?? string.Empty);
            
            //httpContext.Response.Headers.Add("span-id", activity.SpanId.ToString() ?? string.Empty);
        }
    }
}