namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class ListUsersHandler(IUnitOfWork unitOfWork, ILogger<ListUsersHandler> logger) : ICommandHandler<ListUsersCommand, ListUsersResult>
{
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = Common.Security.UserAdministration.Policy)]
    public async Task<ListUsersResult> ExecuteAsync(ListUsersCommand command, CancellationToken cancellationToken)
    {
        // Invalid or missing paging is InvalidArgument, the same rule REST applies.
        var pagination = PaginationParams.Require(command.Pagination);
        logger.LogInformation("Fetching all users with pagination page {PageNumber}, size {PageSize}", pagination.PageNumber, pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.User>();
        var response = await repository.GetAllAsync(pagination, cancellationToken);

        logger.LogInformation("Fetched {Count} user records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListUsersResult
        {
            Success = true,
            Message = "Users listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");

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
