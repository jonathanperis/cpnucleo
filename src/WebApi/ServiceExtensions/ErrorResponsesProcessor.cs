using System.Globalization;
using Namotion.Reflection;
using NJsonSchema;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace WebApi.ServiceExtensions;

/// <summary>
/// Documents the error statuses an endpoint can return, all with the <see cref="ApiErrorResponse"/>
/// envelope: 401 and 429/500 everywhere; 404 except for creates and lists; 403 (outside the caller's
/// own account) and 409 for writes. Validators add 400 and policies add 403 on their own.
/// This is a document processor rather than a FastEndpoints endpoint configurator because
/// UseFastEndpoints settings apply once per process, and the integration tests host several APIs.
/// </summary>
internal sealed class ErrorResponsesProcessor : IOperationProcessor
{
    private static readonly Dictionary<string, string> Defaults = new()
    {
        ["401"] = "Unauthorized",
        ["403"] = "Forbidden",
        ["404"] = "Not Found",
        ["409"] = "Conflict",
        ["429"] = "Too Many Requests",
        ["500"] = "Server Error",
    };

    public bool Process(OperationProcessorContext context)
    {
        var endpoint = (context as AspNetCoreOperationProcessorContext)?.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<EndpointDefinition>().FirstOrDefault();
        if (endpoint?.EndpointType.Namespace is not { } ns) return true;

        var feature = ns.Split('.')[^1];
        var write = !endpoint.Verbs.Contains("GET");
        var statuses = new List<string> { "401" };
        if (!feature.StartsWith("List", StringComparison.Ordinal) && !feature.StartsWith("Create", StringComparison.Ordinal)) statuses.Add("404");
        if (write && !ns.Contains(".Account.", StringComparison.Ordinal)) statuses.Add("403");
        if (write) statuses.Add("409");
        statuses.AddRange(["429", "500"]);

        var responses = context.OperationDescription.Operation.Responses;
        var texts = endpoint.EndpointSummary?.Responses;
        foreach (var status in statuses.Union(responses.Keys.Where(status => status == "403")))
        {
            if (!responses.TryGetValue(status, out var response))
                responses[status] = response = new OpenApiResponse { Description = Defaults[status] };
            // Summary texts (s.Responses) describe statuses this processor adds after FastEndpoints' own.
            if (texts is not null && texts.TryGetValue(int.Parse(status, CultureInfo.InvariantCulture), out var text)) response.Description = text;
            response.Content["application/json"] = new OpenApiMediaType
            {
                Schema = context.SchemaGenerator.GenerateWithReferenceAndNullability<JsonSchema>(
                    typeof(ApiErrorResponse).ToContextualType(), false, context.SchemaResolver)
            };
        }
        return true;
    }
}
