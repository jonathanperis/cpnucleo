using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.ResourceDetectors.Container;
using OpenTelemetry.ResourceDetectors.Host;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebApi.Configurations;

public static class ConfigureOpenTelemetryOptions
{
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        Action<ResourceBuilder> appResourceBuilder =
            resource => resource
                .AddDetector(new ContainerResourceDetector())
                .AddDetector(new HostDetector());
                
        builder.Logging.AddOpenTelemetry(x =>
        {
            x.IncludeScopes = true;
            x.IncludeFormattedMessage = true;
        });

        builder.Services.AddOpenTelemetry()
            .WithMetrics(x =>
            {
                x.AddRuntimeInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http");

                x.AddAspNetCoreInstrumentation()
                    .AddProcessInstrumentation();
            })
            .WithTracing(x =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    x.SetSampler<AlwaysOnSampler>();
                }
                
                x.AddAspNetCoreInstrumentation()
                    .AddGrpcClientInstrumentation()
                    .AddHttpClientInstrumentation();
            })
            .ConfigureResource(appResourceBuilder);

        builder.AddOpenTelemetryExporters();
        
        return builder;
    }

    private static IHostApplicationBuilder AddOpenTelemetryExporters(this IHostApplicationBuilder builder)
    {
        var useOtlpExporter = !string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]);
        
        if (useOtlpExporter)
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(logging => logging.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter());
            builder.Services.ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }

        builder.Services.AddOpenTelemetry().WithMetrics(x => x.AddPrometheusExporter());
        
        return builder;
    }
}