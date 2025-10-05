namespace WebApi.Endpoints.UserAssignment.ListUserAssignments;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/userAssignments");
        Description(x => x.WithTags("UserAssignments"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of userAssignments";
            s.Description = "Fetches userAssignments based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all userAssignments with pagination page {PageNumber}, size {PageSize}", request.Pagination?.PageNumber, request.Pagination?.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
        var response = await repository.GetAllAsync(request.Pagination);

        Logger.LogInformation("Fetched {Count} userAssignment records", response.Data?.Count() ?? 0);
        Logger.LogInformation("Mapping entities to DTOs.");

        Response.Result = MapToPaginatedDto(response);

        Logger.LogInformation("Mapping complete, setting response result.");
        Logger.LogInformation("Service completed successfully.");

        await SendOkAsync(Response, cancellation: cancellationToken);
    }

    private static PaginatedResult<UserAssignmentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.UserAssignment?> result)
    {
        return new PaginatedResult<UserAssignmentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
