namespace WebApi.Endpoints.Organization.ListOrganizations;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/organizations");
        Description(x => x.WithTags("Organizations"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of organizations";
            s.Description = "Fetches organizations based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all organizations with pagination page {PageNumber}, size {PageSize}", request.Pagination?.PageNumber, request.Pagination?.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
        var response = await repository.GetAllAsync(request.Pagination);

        Logger.LogInformation("Fetched {Count} organization records", response.Data?.Count() ?? 0);
        Logger.LogInformation("Mapping entities to DTOs.");

        Response.Result = MapToPaginatedDto(response);

        Logger.LogInformation("Mapping complete, setting response result.");
        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }

    private static PaginatedResult<OrganizationDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Organization?> result)
    {
        return new PaginatedResult<OrganizationDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
