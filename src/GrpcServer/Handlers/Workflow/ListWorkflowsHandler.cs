namespace GrpcServer.Handlers.Workflow;

// Dapper Repository Advanced
public sealed class ListWorkflowsHandler(IUnitOfWork unitOfWork, ILogger<ListWorkflowsHandler> logger) : ICommandHandler<ListWorkflowsCommand, ListWorkflowsResult>
{
    public async Task<ListWorkflowsResult> ExecuteAsync(ListWorkflowsCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all workflows with pagination page {PageNumber}, size {PageSize}", command.Pagination.PageNumber, command.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
        var response = await repository.GetAllAsync(command.Pagination);

        logger.LogInformation("Fetched {Count} workflow records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListWorkflowsResult
        {
            Success = true,
            Message = "Workflows listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
    }

    private static PaginatedResult<WorkflowDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Workflow?> result)
    {
        return new PaginatedResult<WorkflowDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}