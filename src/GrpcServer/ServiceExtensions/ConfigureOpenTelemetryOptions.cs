namespace GrpcServer.ServiceExtensions;

public static class ConfigureOpenTelemetryOptions
{
    private const string DefaultOtlpEndpoint = "http://localhost:4317";

    public static IHostApplicationBuilder ConfigureOpenTelemetry(this IHostApplicationBuilder builder)
    {
        builder.Services.Configure<AspNetCoreTraceInstrumentationOptions>(
            builder.Configuration.GetSection("AspNetCoreInstrumentation"));

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(resource => ConfigureResource(builder, resource))
            .WithTracing(tracing =>
            {
                tracing
                    .SetSampler(new AlwaysOnSampler())
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequest = (activity, request) =>
                        {
                            activity.SetTag("http.request.method", request.Method);
                            activity.SetTag("http.request.host", request.Host.Value);
                            activity.SetTag("http.request.scheme", request.Scheme);
                            activity.SetTag("http.request.protocol", request.Protocol);
                            activity.SetTag("http.request.path", request.Path.Value);
                            activity.SetTag("http.request.query_string_length", request.QueryString.Value?.Length ?? 0);
                            activity.SetTag("user_agent.original", request.Headers.UserAgent.ToString());
                        };
                        options.EnrichWithHttpResponse = (activity, response) =>
                        {
                            activity.SetTag("http.response.content_length", response.ContentLength);
                            activity.SetTag("http.response.content_type", response.ContentType);
                        };
                        options.EnrichWithException = (activity, exception) =>
                        {
                            activity.SetTag("exception.type", exception.GetType().FullName);
                            activity.SetTag("exception.message", exception.Message);
                            activity.SetTag("exception.stacktrace", exception.StackTrace);
                        };
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequestMessage = (activity, request) =>
                        {
                            activity.SetTag("http.request.method", request.Method.Method);
                            activity.SetTag("http.request.host", request.RequestUri?.Host);
                            activity.SetTag("http.request.path", request.RequestUri?.AbsolutePath);
                        };
                        options.EnrichWithHttpResponseMessage = (activity, response) =>
                        {
                            activity.SetTag("http.response.content_length", response.Content.Headers.ContentLength);
                            activity.SetTag("http.response.content_type", response.Content.Headers.ContentType?.MediaType);
                        };
                        options.EnrichWithException = (activity, exception) =>
                        {
                            activity.SetTag("exception.type", exception.GetType().FullName);
                            activity.SetTag("exception.message", exception.Message);
                            activity.SetTag("exception.stacktrace", exception.StackTrace);
                        };
                    })
                    .AddNpgsql()
                    .AddOtlpExporter(options => ConfigureOtlpExporter(builder, options));
            })
            .WithMetrics(metrics =>
            {
                metrics
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddProcessInstrumentation()
                    .AddMeter(
                        "Microsoft.AspNetCore.RateLimiting",
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        "System.Net.NameResolution",
                        "Npgsql")
                    .AddNpgsqlInstrumentation(_ => { })
                    .AddOtlpExporter(options => ConfigureOtlpExporter(builder, options));
            });

        builder.Logging.AddConsole();
        builder.Logging.AddOpenTelemetry(options =>
        {
            var loggingSection = builder.Configuration.GetSection("Logging:OpenTelemetry");
            options.IncludeFormattedMessage = loggingSection.GetValue("IncludeFormattedMessage", true);
            options.IncludeScopes = loggingSection.GetValue("IncludeScopes", true);
            options.ParseStateValues = loggingSection.GetValue("ParseStateValues", true);

            var resourceBuilder = ResourceBuilder.CreateDefault();
            ConfigureResource(builder, resourceBuilder);
            options.SetResourceBuilder(resourceBuilder);

            options.AddOtlpExporter(otlpOptions => ConfigureOtlpExporter(builder, otlpOptions));
        });

        return builder;
    }

    private static void ConfigureResource(IHostApplicationBuilder builder, ResourceBuilder resource)
    {
        var assembly = typeof(Program).Assembly.GetName();
        var serviceName = builder.Configuration.GetValue("ServiceName", "GrpcServer-Cpnucleo");
        var environmentName = builder.Environment.EnvironmentName;

        resource
            .AddService(
                serviceName: serviceName,
                serviceNamespace: "cpnucleo",
                serviceVersion: assembly.Version?.ToString() ?? "unknown",
                serviceInstanceId: Environment.MachineName)
            .AddAttributes(new Dictionary<string, object>
            {
                ["deployment.environment"] = environmentName,
                ["host.name"] = Environment.MachineName,
                ["process.id"] = Environment.ProcessId,
                ["process.runtime.name"] = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                ["os.description"] = System.Runtime.InteropServices.RuntimeInformation.OSDescription,
                ["cpnucleo.project"] = "grpcserver"
            });
    }

    private static void ConfigureOtlpExporter(IHostApplicationBuilder builder, OpenTelemetry.Exporter.OtlpExporterOptions options)
    {
        options.Endpoint = new Uri(builder.Configuration.GetValue("OTEL_EXPORTER_OTLP_ENDPOINT", DefaultOtlpEndpoint));
    }
}
