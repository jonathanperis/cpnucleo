namespace WebApi.ServiceExtensions;

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
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequest = (activity, request) =>
                        {
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
                        };
                    })
                    .AddHttpClientInstrumentation(options =>
                    {
                        options.RecordException = true;
                        options.EnrichWithHttpRequestMessage = (activity, request) =>
                        {
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
                        };
                    })
                    .AddEntityFrameworkCoreInstrumentation(options =>
                    {
                        options.EnrichWithIDbCommand = (activity, command) =>
                        {
                            activity.SetTag("db.system", "postgresql");
                            activity.SetTag("db.name", command.Connection?.Database);
                            activity.SetTag("db.command.timeout", command.CommandTimeout);
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
                        "Microsoft.AspNetCore.Hosting",
                        "Microsoft.AspNetCore.Server.Kestrel",
                        "System.Net.Http",
                        "System.Net.NameResolution",
                        "Npgsql")
                    .AddNpgsqlInstrumentation(_ => { })
                    .AddOtlpExporter(options => ConfigureOtlpExporter(builder, options));
            });

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

        if (builder.Environment.IsDevelopment())
        {
            builder.Logging.AddConsole();
        }

        return builder;
    }

    private static void ConfigureResource(IHostApplicationBuilder builder, ResourceBuilder resource)
    {
        var assembly = typeof(Program).Assembly.GetName();
        var serviceName = builder.Configuration.GetValue("ServiceName", assembly.Name ?? "cpnucleo-service");
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
                ["cpnucleo.project"] = "webapi"
            });
    }

    private static void ConfigureOtlpExporter(IHostApplicationBuilder builder, OpenTelemetry.Exporter.OtlpExporterOptions options)
    {
        options.Endpoint = new Uri(builder.Configuration.GetValue("OTEL_EXPORTER_OTLP_ENDPOINT", DefaultOtlpEndpoint));
    }
}
