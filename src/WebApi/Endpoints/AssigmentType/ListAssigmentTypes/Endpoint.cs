namespace WebApi.Endpoints.AssignmentType.ListAssignmentTypes;

// Dapper Repository Advanced
public class Endpoint(IUnitOfWork unitOfWork) : Endpoint<Request, Response>
{
    public override void Configure()
    {
        Get("/api/assignmentTypes");
        Description(x => x.WithTags("AssignmentTypes"));
        AllowAnonymous();

        Summary(s =>
        {
            s.Summary = "Retrieve a paginated list of assignmentTypes";
            s.Description = "Fetches assignmentTypes based on pagination parameters, maps entities to DTOs, and returns paginated results with metadata (total count, page number, and page size).";
        });
    }

    public override async Task HandleAsync(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Service started processing request.");
        Logger.LogInformation("Fetching all assignmentTypes with pagination page {PageNumber}, size {PageSize}", request.Pagination?.PageNumber, request.Pagination?.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
        var response = await repository.GetAllAsync(request.Pagination);

        Logger.LogInformation("Fetched {Count} assignmentType records", response.Data?.Count() ?? 0);
        Logger.LogInformation("Mapping entities to DTOs.");

        Response.Result = MapToPaginatedDto(response);

        Logger.LogInformation("Mapping complete, setting response result.");
        Logger.LogInformation("Service completed successfully.");

        await Send.OkAsync(Response, cancellationToken);
    }

    private static PaginatedResult<AssignmentTypeDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.AssignmentType?> result)
    {
        return new PaginatedResult<AssignmentTypeDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
