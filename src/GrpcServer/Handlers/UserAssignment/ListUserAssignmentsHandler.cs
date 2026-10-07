namespace GrpcServer.Handlers.UserAssignment;

// Dapper Repository Advanced
public sealed class ListUserAssignmentsHandler(IUnitOfWork unitOfWork, ILogger<ListUserAssignmentsHandler> logger) : ICommandHandler<ListUserAssignmentsCommand, ListUserAssignmentsResult>
{
    public async Task<ListUserAssignmentsResult> ExecuteAsync(ListUserAssignmentsCommand command, CancellationToken cancellationToken)
    {
        // Invalid or missing paging is InvalidArgument, the same rule REST applies.
        var pagination = PaginationParams.Require(command.Pagination);
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all userAssignments with pagination page {PageNumber}, size {PageSize}", pagination.PageNumber, pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
        var response = await repository.GetAllAsync(pagination, cancellationToken);

        logger.LogInformation("Fetched {Count} userAssignment records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListUserAssignmentsResult
        {
            Success = true,
            Message = "UserAssignments listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
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
