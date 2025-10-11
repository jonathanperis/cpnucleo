namespace GrpcServer.Handlers.AssignmentType;

// Dapper Repository Advanced
public sealed class ListAssignmentTypesHandler(IUnitOfWork unitOfWork, ILogger<ListAssignmentTypesHandler> logger) : ICommandHandler<ListAssignmentTypesCommand, ListAssignmentTypesResult>
{
    public async Task<ListAssignmentTypesResult> ExecuteAsync(ListAssignmentTypesCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all assignmentTypes with pagination page {PageNumber}, size {PageSize}", command.Pagination.PageNumber, command.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
        var response = await repository.GetAllAsync(command.Pagination);

        logger.LogInformation("Fetched {Count} assignmentType records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListAssignmentTypesResult
        {
            Success = true,
            Message = "AssignmentTypes listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
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