namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class ListUsersHandler(IUnitOfWork unitOfWork, ILogger<ListUsersHandler> logger) : ICommandHandler<ListUsersCommand, ListUsersResult>
{
    public async Task<ListUsersResult> ExecuteAsync(ListUsersCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all users with pagination page {PageNumber}, size {PageSize}", command.Pagination.PageNumber, command.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.User>();
        var response = await repository.GetAllAsync(command.Pagination);

        logger.LogInformation("Fetched {Count} user records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListUsersResult
        {
            Success = true,
            Message = "Users listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
    }

    private static PaginatedResult<UserDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.User?> result)
    {
        return new PaginatedResult<UserDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}