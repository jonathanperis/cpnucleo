using System.Globalization;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization.Metadata;
using Domain.Models;
using FastEndpoints;
using Infrastructure.Http;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace OpenApiLab;

/// <summary>
/// Microsoft.AspNetCore.OpenApi ports of WebApi's two NSwag operation processors
/// (src/WebApi/ServiceExtensions/PaginationQueryProcessor.cs and ErrorResponsesProcessor.cs), plus a
/// schema id function equivalent to NSwag's output names. Same rules, different object model.
/// </summary>
internal static class Transformers
{
    private static EndpointDefinition? Endpoint(OpenApiOperationTransformerContext context) =>
        context.Description.ActionDescriptor.EndpointMetadata.OfType<EndpointDefinition>().FirstOrDefault();

    /// <summary>
    /// FastEndpoints.OpenApi already flattens <see cref="PaginationParams"/> into one query parameter per
    /// property, so unlike the NSwag processor this one replaces those keys rather than one object parameter:
    /// it adds descriptions, bounds and defaults and keeps only the filters the listed rows support.
    /// </summary>
    public sealed class PaginationQuery : IOpenApiOperationTransformer
    {
        private static readonly string[] RelationColumns = ["OrganizationId", "ProjectId", "AssignmentId", "UserId", "WorkflowId"];

        private static readonly HashSet<string> Keys = typeof(PaginationParams).GetProperties()
            .Where(property => property.CanWrite)
            .Select(property => PaginationParams.FieldOf(property.Name))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        public Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            var endpoint = Endpoint(context);
            if (endpoint?.ReqDtoType.GetProperties().Any(property => property.PropertyType == typeof(PaginationParams)) != true)
                return Task.CompletedTask;

            var parameters = operation.Parameters ??= [];
            foreach (var parameter in parameters.Where(parameter => parameter.In == ParameterLocation.Query && Keys.Contains(parameter.Name ?? "")).ToList())
                parameters.Remove(parameter);

            Add(parameters, "pageNumber", "Page to return, starting at 1.",
                Integer(minimum: 1, maximum: PaginationParams.MaximumPageNumber, @default: 1));
            Add(parameters, "pageSize", $"Rows per page, 1 to {PaginationParams.MaximumPageSize}.",
                Integer(minimum: 1, maximum: PaginationParams.MaximumPageSize, @default: 10));
            Add(parameters, "search", "Case-insensitive text search; wildcard characters match literally.",
                new OpenApiSchema { Type = JsonSchemaType.String, MaxLength = PaginationParams.MaximumSearchLength });
            Add(parameters, "sortColumn", "A persisted property of the listed record.",
                new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create("Id") });
            Add(parameters, "sortOrder", "Sort direction.",
                new OpenApiSchema { Type = JsonSchemaType.String, Default = JsonValue.Create("ASC"), Enum = [JsonValue.Create("ASC"), JsonValue.Create("DESC")] });
            Add(parameters, "ids", $"Comma-separated UUIDs (at most {PaginationParams.MaximumPageSize}) restricting the result.",
                new OpenApiSchema { Type = JsonSchemaType.String });

            if (ListedEntity(endpoint.EndpointType) is not { } entity) return Task.CompletedTask;
            foreach (var column in RelationColumns.Where(column => PaginationParams.SupportsRelation(entity, column)))
                Add(parameters, PaginationParams.FieldOf(column), $"Only rows whose {column} is this UUID.",
                    new OpenApiSchema { Type = JsonSchemaType.String, Format = "uuid" });
            if (PaginationParams.DateColumnsOf(entity) is var (start, end))
            {
                var applies = start == end ? start : $"the {start}..{end} period";
                Add(parameters, "dateFrom", $"Inclusive lower bound (ISO-8601, UTC) of {applies}.", new OpenApiSchema { Type = JsonSchemaType.String });
                Add(parameters, "dateTo", $"Exclusive upper bound (ISO-8601, UTC) of {applies}.", new OpenApiSchema { Type = JsonSchemaType.String });
            }
            return Task.CompletedTask;
        }

        private static OpenApiSchema Integer(int minimum, int maximum, int @default) => new()
        {
            Type = JsonSchemaType.Integer,
            Format = "int32",
            Minimum = minimum.ToString(CultureInfo.InvariantCulture),
            Maximum = maximum.ToString(CultureInfo.InvariantCulture),
            Default = JsonValue.Create(@default)
        };

        /// <summary>The entity a list endpoint returns, from its feature folder (<c>WebApi.Endpoints.&lt;Entity&gt;.&lt;Feature&gt;</c>).</summary>
        private static Type? ListedEntity(Type endpointType) =>
            endpointType.Namespace?.Split('.') is { Length: 4 } segments
                ? typeof(Domain.Entities.Project).Assembly.GetType($"Domain.Entities.{segments[2]}")
                : null;

        private static void Add(IList<IOpenApiParameter> parameters, string name, string description, OpenApiSchema schema) =>
            parameters.Add(new OpenApiParameter { Name = name, In = ParameterLocation.Query, Description = description, Schema = schema, Required = false });
    }

    /// <summary>Same status rules as ErrorResponsesProcessor; every error response references the envelope.</summary>
    public sealed class ErrorResponses : IOpenApiOperationTransformer
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

        public async Task TransformAsync(OpenApiOperation operation, OpenApiOperationTransformerContext context, CancellationToken cancellationToken)
        {
            if (Endpoint(context) is not { EndpointType.Namespace: { } ns } endpoint) return;

            var feature = ns.Split('.')[^1];
            var write = !endpoint.Verbs.Contains("GET");
            var statuses = new List<string> { "401" };
            if (!feature.StartsWith("List", StringComparison.Ordinal) && !feature.StartsWith("Create", StringComparison.Ordinal)) statuses.Add("404");
            if (write && !ns.Contains(".Account.", StringComparison.Ordinal)) statuses.Add("403");
            if (write) statuses.Add("409");
            statuses.AddRange(["429", "500"]);

            // The envelope is already a component when a validator's 400 references it; otherwise create it.
            var document = context.Document!;
            const string envelope = nameof(ApiErrorResponse);
            if (document.Components?.Schemas?.ContainsKey(envelope) != true)
                document.AddComponent(envelope, await context.GetOrCreateSchemaAsync(typeof(ApiErrorResponse), null, cancellationToken));

            var responses = operation.Responses ??= [];
            var texts = endpoint.EndpointSummary?.Responses;
            foreach (var status in statuses.Union(responses.Keys.Where(status => status == "403")).ToList())
            {
                if (!responses.TryGetValue(status, out var existing) || existing is not OpenApiResponse response)
                    responses[status] = response = new OpenApiResponse { Description = Defaults[status] };
                if (texts is not null && texts.TryGetValue(int.Parse(status, CultureInfo.InvariantCulture), out var text)) response.Description = text;
                response.Content ??= new Dictionary<string, OpenApiMediaType>();
                response.Content["application/json"] = new OpenApiMediaType { Schema = new OpenApiSchemaReference(envelope, document) };
            }
        }
    }

    /// <summary>
    /// NSwag names WebApi's per-feature <c>Request</c>/<c>Response</c> classes after their feature folder
    /// (<c>CreateAppointmentRequest</c>) and generics as <c>PaginatedResultOfProjectDto</c>; collections stay inline.
    /// </summary>
    public static string? SchemaId(JsonTypeInfo typeInfo)
    {
        if (OpenApiOptions.CreateDefaultSchemaReferenceId(typeInfo) is null) return null;
        if (typeInfo.Kind is JsonTypeInfoKind.Enumerable or JsonTypeInfoKind.Dictionary) return null;
        return Name(typeInfo.Type);
    }

    private static string Name(Type type)
    {
        if (type.Namespace?.StartsWith("WebApi.Endpoints.", StringComparison.Ordinal) == true && type.Name is "Request" or "Response")
            return type.Namespace.Split('.')[^1] + type.Name;
        if (!type.IsGenericType) return type.Name;
        var baseName = type.Name[..type.Name.IndexOf('`')];
        return $"{baseName}Of{string.Join("Of", type.GetGenericArguments().Select(Name))}";
    }
}
