namespace GrpcServer.Handlers.UserAssignment;

// Dapper Repository Advanced
public sealed class ListUserAssignmentsHandler(IUnitOfWork unitOfWork, ILogger<ListUserAssignmentsHandler> logger) : ICommandHandler<ListUserAssignmentsCommand, ListUserAssignmentsResult>
{
    public async Task<ListUserAssignmentsResult> ExecuteAsync(ListUserAssignmentsCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all userAssignments with pagination page {PageNumber}, size {PageSize}", command.Pagination.PageNumber, command.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
        var response = await repository.GetAllAsync(command.Pagination);

        logger.LogInformation("Fetched {Count} userAssignment records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListUserAssignmentsResult
        {
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