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
using ProjectX.Core.Observability.Logging;
using OpenTelemetry.Logs;

namespace ProjectX.Core.Observability;

public static class ObservabilitySetup
{
    public static WebApplicationBuilder AddObservabilityServices(this WebApplicationBuilder app)
    {
        app
        .AddLogging()
        .AddOpenTelemetry()
        .Services
        .AddTracer()
        .AddTracerBehaviour();

        return app;
    }

    public static IServiceCollection AddTracer(this IServiceCollection services)
        => services.AddSingleton<ITracer, CoreTracer>();

    public static IServiceCollection AddTracerBehaviour(this IServiceCollection services)
        => services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracerBehaviour<,>));

    public static WebApplicationBuilder AddLogging(this WebApplicationBuilder app)
    {
        app.Services.AddLogging(builder =>
        {
            var loggerConfig = new LoggerConfiguration()
                 .Enrich.WithProperty("Application", app.Environment.ApplicationName)
                 .Enrich.WithProperty("Environment", app.Environment.EnvironmentName)
                 .MinimumLevel.Information()
                 .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                 .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Warning)
                 .Enrich.FromLogContext()
                 .Enrich.With<CorrelationLogEventEnricher>()
                 .Enrich.WithSpan()
                 .WriteTo.Console()
                 .WriteTo.Seq(EnvironmentVariables.SEQ_URI)
                 //.WriteTo.Elasticsearch()
                 .WriteTo.File(
                    path: "logs/log.txt", 
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Information,
                    //retainedFileCountLimit: max number of log files
                    retainedFileCountLimit: 30)
                 .ReadFrom.Configuration(app.Configuration);

            Log.Logger = loggerConfig.CreateLogger();

            builder.ClearProviders()
                   // NOTE: AddOpenTelemetry Logging need to be more investigated
                   //.AddOpenTelemetry(options =>
                   //{
                   //    //options.SetResourceBuilder(appResourceBuilder);
                   //    //options.AddConsoleExporter();
                   //})
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
            .AddOpenTelemetry()
            .WithTracing(tracing =>
            {
                tracing
                    .AddSource(CoreTracer.Name)
                    .AddConsoleExporter(o =>
                    {
                        o.Targets = ConsoleExporterOutputTargets.Console;
                    })
                    .AddJaegerExporter(o =>
                    {
                        // ENV: OTEL_EXPORTER_JAEGER_ENDPOINT
                        // https://github.com/open-telemetry/opentelemetry-dotnet/blob/main/src/OpenTelemetry.Exporter.Jaeger/README.md
                        o.AgentHost = EnvironmentVariables.JAEGER_HOST;
                        o.AgentPort = EnvironmentVariables.JAEGER_PORT;
                    })
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        // Exclude swagger
                        options.Filter = c =>
                             c.Request.Path.Value != null &&
                            !c.Request.Path.Value.Contains("swagger") &&
                            !c.Request.Path.Value.Contains("_vs/browserLink") &&
                            !c.Request.Path.Value.Contains("_framework/aspnetcore-browser-refresh.js");
                        
                        options.RecordException = true;
                        
                        options.EnrichWithHttpResponse = (act, respo) => 
                        {
                            ExtractContextFromResponse(act, respo);
                        };
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.FilterHttpRequestMessage = message =>
                            message?.RequestUri != null &&
                            !message.RequestUri.Host.Contains("visualstudio");
                    })
                    .AddSqlClientInstrumentation(options =>
                    {
                        options.EnableConnectionLevelAttributes = true;
                        options.SetDbStatementForStoredProcedure = true;
                        options.SetDbStatementForText = true;
                        options.RecordException = true;
                    })
                    .AddRedisInstrumentation()
                    .SetSampler(new AlwaysOnSampler());
            })
            .ConfigureResource(builder =>
            {
                builder
                    .AddTelemetrySdk()
                    .AddService(serviceName, 
                        serviceVersion: serviceVersion,
                        serviceInstanceId: Environment.MachineName);
            });

        return app;
    }

    private static void ExtractContextFromResponse(Activity activity, HttpResponse response)
    {
        var httpContext = response?.HttpContext;
        var user = httpContext?.User;

        if (activity == null || user == null) return;

        var identityId = user.Claims.FirstOrDefault(c => c.Type == ApplicationClaimTypes.IdentityId)?.Value;

        if (!string.IsNullOrWhiteSpace(identityId))
        {
            activity.SetTag("identity.id", identityId);
        }

        var identityRole = user.Claims.FirstOrDefault(c => c.Type == ApplicationClaimTypes.IdentityRole)?.Value;

        if (!string.IsNullOrWhiteSpace(identityRole))
        {
            activity.SetTag("identity.role", identityRole);
        }

        //httpContext.Response.Headers.Add("trace-id", activity.TraceId.ToString() ?? string.Empty);

        //httpContext.Response.Headers.Add("span-id", activity.SpanId.ToString() ?? string.Empty);
    }
}