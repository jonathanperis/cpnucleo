using NJsonSchema;
using NSwag;
using NSwag.Generation.AspNetCore;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace WebApi.ServiceExtensions;

/// <summary>
/// List endpoints bind <see cref="PaginationParams"/> from flat query keys, but NSwag describes the
/// property as one object parameter. This replaces it with the keys clients send, listing only the
/// relation and date filters the listed rows support (any other filter is a 400).
/// </summary>
internal sealed class PaginationQueryProcessor : IOperationProcessor
{
    private static readonly string[] RelationColumns = ["OrganizationId", "ProjectId", "AssignmentId", "UserId", "WorkflowId"];

    public bool Process(OperationProcessorContext context)
    {
        var parameters = context.OperationDescription.Operation.Parameters;
        var pagination = parameters.FirstOrDefault(parameter => parameter.Kind == OpenApiParameterKind.Query && parameter.Name == "pagination");
        if (pagination is null) return true;
        parameters.Remove(pagination);

        Add(parameters, "pageNumber", "Page to return, starting at 1.",
            new JsonSchema { Type = JsonObjectType.Integer, Format = "int32", Minimum = 1, Maximum = PaginationParams.MaximumPageNumber, Default = 1 });
        Add(parameters, "pageSize", $"Rows per page, 1 to {PaginationParams.MaximumPageSize}.",
            new JsonSchema { Type = JsonObjectType.Integer, Format = "int32", Minimum = 1, Maximum = PaginationParams.MaximumPageSize, Default = 10 });
        Add(parameters, "search", "Case-insensitive text search; wildcard characters match literally.",
            new JsonSchema { Type = JsonObjectType.String, MaxLength = PaginationParams.MaximumSearchLength });
        Add(parameters, "sortColumn", "A persisted property of the listed record.",
            new JsonSchema { Type = JsonObjectType.String, Default = "Id" });
        var sortOrder = new JsonSchema { Type = JsonObjectType.String, Default = "ASC" };
        sortOrder.Enumeration.Add("ASC");
        sortOrder.Enumeration.Add("DESC");
        Add(parameters, "sortOrder", "Sort direction.", sortOrder);
        Add(parameters, "ids", $"Comma-separated UUIDs (at most {PaginationParams.MaximumPageSize}) restricting the result.",
            new JsonSchema { Type = JsonObjectType.String });

        if (ListedEntity(context) is not { } entity) return true;
        foreach (var column in RelationColumns.Where(column => PaginationParams.SupportsRelation(entity, column)))
            Add(parameters, PaginationParams.FieldOf(column), $"Only rows whose {column} is this UUID.",
                new JsonSchema { Type = JsonObjectType.String, Format = JsonFormatStrings.Guid });
        if (PaginationParams.DateColumnsOf(entity) is var (start, end))
        {
            var applies = start == end ? start : $"the {start}..{end} period";
            Add(parameters, "dateFrom", $"Inclusive lower bound (ISO-8601, UTC) of {applies}.", new JsonSchema { Type = JsonObjectType.String });
            Add(parameters, "dateTo", $"Exclusive upper bound (ISO-8601, UTC) of {applies}.", new JsonSchema { Type = JsonObjectType.String });
        }
        return true;
    }

    /// <summary>The entity a list endpoint returns, from its feature folder (<c>WebApi.Endpoints.&lt;Entity&gt;.&lt;Feature&gt;</c>).</summary>
    private static Type? ListedEntity(OperationProcessorContext context)
    {
        var endpointType = (context as AspNetCoreOperationProcessorContext)?.ApiDescription.ActionDescriptor.EndpointMetadata
            .OfType<EndpointDefinition>().FirstOrDefault()?.EndpointType;
        var segments = endpointType?.Namespace?.Split('.');
        return segments is { Length: 4 } ? typeof(Domain.Entities.Project).Assembly.GetType($"Domain.Entities.{segments[2]}") : null;
    }

    private static void Add(ICollection<OpenApiParameter> parameters, string name, string description, JsonSchema schema) =>
        parameters.Add(new OpenApiParameter { Name = name, Kind = OpenApiParameterKind.Query, Description = description, Schema = schema, IsRequired = false });
}
