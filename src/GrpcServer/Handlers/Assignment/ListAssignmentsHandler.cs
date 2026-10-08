namespace GrpcServer.Handlers.Assignment;

// Dapper Repository Advanced
public sealed class ListAssignmentsHandler(IUnitOfWork unitOfWork, ILogger<ListAssignmentsHandler> logger) : ICommandHandler<ListAssignmentsCommand, ListAssignmentsResult>
{
    public async Task<ListAssignmentsResult> ExecuteAsync(ListAssignmentsCommand command, CancellationToken cancellationToken)
    {
        // Invalid or missing paging is InvalidArgument, the same rule REST applies.
        var pagination = PaginationParams.Require(command.Pagination);
        logger.LogInformation("Fetching all assignments with pagination page {PageNumber}, size {PageSize}", pagination.PageNumber, pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
        var response = await repository.GetAllAsync(pagination, cancellationToken);

        logger.LogInformation("Fetched {Count} assignment records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListAssignmentsResult
        {
            Success = true,
            Message = "Assignments listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");

        return result;
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
