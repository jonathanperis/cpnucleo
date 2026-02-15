namespace WebApi.Endpoints.Impediment.ListImpediments;

// EF Core
public class Endpoint(IApplicationDbContext dbContext) : Endpoint<Request, Response>
{
    // Cache reflection results to avoid repeated GetProperties calls
    private static readonly Lazy<HashSet<string>> CachedPropertyNames = new(() =>
    {
        var properties = typeof(Domain.Entities.Impediment).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return new HashSet<string>(properties.Select(p => p.Name), StringComparer.OrdinalIgnoreCase);
    });
    
    public override void Configure()
    {
        Get("/api/impediments");
        Description(x => x.WithTags("Impediments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of impediments";
            s.Description = "Fetches impediments based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all impediments with pagination page {PageNumber}, size {PageSize}", request.Pagination.PageNumber, request.Pagination.PageSize);

        var query = dbContext.Impediments?.AsNoTracking().AsQueryable();

        var validSortColumn = ValidateSortColumn(request.Pagination.SortColumn);
        var validSortOrder = request.Pagination.SortOrder?.ToUpper() == "DESC" ? "DESC" : "ASC";

        query = ApplySorting(query, validSortColumn, validSortOrder);

        var totalCount = await query!.CountAsync(cancellationToken);

        var response = await query!
            .Skip(request.Pagination.Offset)
            .Take(request.Pagination.PageSize.GetValueOrDefault())
            .ToListAsync(cancellationToken);

        Logger.LogInformation("Fetched {Count} impediment records", response.Count);
        Logger.LogInformation("Mapping entities to DTOs.");

        Response.Result = new PaginatedResult<ImpedimentDto?>
        {
            Data = response.Select(x => x.MapToDto()).ToList(),
            TotalCount = totalCount,
            PageNumber = request.Pagination.PageNumber.GetValueOrDefault(),
            PageSize = request.Pagination.PageSize.GetValueOrDefault()
        };

        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }

    private static string ValidateSortColumn(string? column)
    {
        return !string.IsNullOrWhiteSpace(column) && CachedPropertyNames.Value.Contains(column)
            ? column
            : "Id";
    }

    private static IQueryable<Domain.Entities.Impediment>? ApplySorting(IQueryable<Domain.Entities.Impediment>? query, string sortColumn, string sortOrder)
    {
        return query?.OrderBy($"{sortColumn} {sortOrder}");
    }
}
