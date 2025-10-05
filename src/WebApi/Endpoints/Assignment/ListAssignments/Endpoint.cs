namespace WebApi.Endpoints.Assignment.ListAssignments;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/assignments");
        Description(x => x.WithTags("Assignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of assignments";
            s.Description = "Fetches assignments based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all assignments with pagination page {PageNumber}, size {PageSize}", request.Pagination?.PageNumber, request.Pagination?.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
        var response = await repository.GetAllAsync(request.Pagination);

        Logger.LogInformation("Fetched {Count} assignment records", response.Data?.Count() ?? 0);
        Logger.LogInformation("Mapping entities to DTOs.");

        Response.Result = MapToPaginatedDto(response);

        Logger.LogInformation("Mapping complete, setting response result.");
        Logger.LogInformation("Service completed successfully.");

        await SendOkAsync(Response, cancellation: cancellationToken);
    }

    private static PaginatedResult<AssignmentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Assignment?> result)
    {
        return new PaginatedResult<AssignmentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
