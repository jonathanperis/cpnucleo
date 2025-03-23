using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebApi.Configurations;

public static class ConfigureOpenTelemetryOptions
{
    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        // Configure OpenTelemetry tracing & metrics with auto-start using the
        // AddOpenTelemetry extension from OpenTelemetry.Extensions.Hosting.
        builder.Services.AddOpenTelemetry()
            .ConfigureResource(ConfigureResource)
            .WithTracing(tpb =>
            {
                tpb
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                // Use IConfiguration binding for AspNetCore instrumentation options.
                builder.Services.Configure<AspNetCoreTraceInstrumentationOptions>(builder.Configuration.GetSection("AspNetCoreInstrumentation"));

                tpb.AddOtlpExporter(otlpOptions =>
                {
                    // Use IConfiguration binding for AspNetCore instrumentation options.
                    otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
                });
            })
            .WithMetrics(mpb =>
            {
                mpb
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()      
                    .AddHttpClientInstrumentation()
                    .AddAspNetCoreInstrumentation();

                mpb.AddOtlpExporter(otlpOptions =>
                {
                    // Use IConfiguration binding for AspNetCore instrumentation options.
                    otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
                });
            });

        // Clear default logging providers used by WebApplication host.
        builder.Logging.ClearProviders();

        // Configure OpenTelemetry Logging.
        builder.Logging.AddOpenTelemetry(options =>
        {
            // Note: See appsettings.json Logging:OpenTelemetry section for configuration.

            var resourceBuilder = ResourceBuilder.CreateDefault();
            ConfigureResource(resourceBuilder);
            options.SetResourceBuilder(resourceBuilder);

            options.AddOtlpExporter(otlpOptions =>
            {
                // Use IConfiguration binding for AspNetCore instrumentation options.
                otlpOptions.Endpoint = new Uri(builder.Configuration.GetValue("Otlp:Endpoint", defaultValue: "http://localhost:4317")!);
            });

            // Add the Console exporter for local debugging.
            options.AddConsoleExporter();    
        });
        
        return builder;

        // Build a resource configuration action to set service information.
        void ConfigureResource(ResourceBuilder r) => 
                r.AddService(serviceName: builder.Configuration.GetValue("ServiceName", defaultValue: "otel-test")!, 
                             serviceVersion: typeof(Program).Assembly.GetName().Version?.ToString() ?? "unknown", 
                             serviceInstanceId: Environment.MachineName);
    }
}