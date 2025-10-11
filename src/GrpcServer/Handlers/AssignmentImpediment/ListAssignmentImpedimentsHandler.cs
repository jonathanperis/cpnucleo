namespace GrpcServer.Handlers.AssignmentImpediment;

// Dapper Repository Advanced
public sealed class ListAssignmentImpedimentsHandler(IUnitOfWork unitOfWork, ILogger<ListAssignmentImpedimentsHandler> logger) : ICommandHandler<ListAssignmentImpedimentsCommand, ListAssignmentImpedimentsResult>
{
    public async Task<ListAssignmentImpedimentsResult> ExecuteAsync(ListAssignmentImpedimentsCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all assignmentImpediments with pagination page {PageNumber}, size {PageSize}", command.Pagination.PageNumber, command.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
        var response = await repository.GetAllAsync(command.Pagination);

        logger.LogInformation("Fetched {Count} assignmentImpediment records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListAssignmentImpedimentsResult
        {
            Success = true,
            Message = "AssignmentImpediments listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
    }

    private static PaginatedResult<AssignmentImpedimentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.AssignmentImpediment?> result)
    {
        return new PaginatedResult<AssignmentImpedimentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}