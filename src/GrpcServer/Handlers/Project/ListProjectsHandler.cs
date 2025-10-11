namespace GrpcServer.Handlers.Project;

// Dapper Repository Advanced
public sealed class ListProjectsHandler(IUnitOfWork unitOfWork, ILogger<ListProjectsHandler> logger) : ICommandHandler<ListProjectsCommand, ListProjectsResult>
{
    public async Task<ListProjectsResult> ExecuteAsync(ListProjectsCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all projects with pagination page {PageNumber}, size {PageSize}", command.Pagination.PageNumber, command.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Project>();
        var response = await repository.GetAllAsync(command.Pagination);

        logger.LogInformation("Fetched {Count} project records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListProjectsResult
        {
            Success = true,
            Message = "Projects listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
    }

    private static PaginatedResult<ProjectDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Project?> result)
    {
        return new PaginatedResult<ProjectDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}