namespace GrpcServer.Handlers.UserProject;

// Dapper Repository Advanced
public sealed class ListUserProjectsHandler(IUnitOfWork unitOfWork, ILogger<ListUserProjectsHandler> logger) : ICommandHandler<ListUserProjectsCommand, ListUserProjectsResult>
{
    public async Task<ListUserProjectsResult> ExecuteAsync(ListUserProjectsCommand command, CancellationToken cancellationToken)
    {
        // Invalid or missing paging is InvalidArgument, the same rule REST applies.
        var pagination = PaginationParams.Require(command.Pagination);
        logger.LogInformation("Fetching all userProjects with pagination page {PageNumber}, size {PageSize}", pagination.PageNumber, pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
        var response = await repository.GetAllAsync(pagination, cancellationToken);

        logger.LogInformation("Fetched {Count} userProject records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListUserProjectsResult
        {
            Success = true,
            Message = "UserProjects listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");

        return result;
    }

    private static PaginatedResult<UserProjectDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.UserProject?> result)
    {
        return new PaginatedResult<UserProjectDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
