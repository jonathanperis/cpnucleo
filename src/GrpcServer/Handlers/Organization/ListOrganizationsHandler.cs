namespace GrpcServer.Handlers.Organization;

// Dapper Repository Advanced
public sealed class ListOrganizationsHandler(IUnitOfWork unitOfWork, ILogger<ListOrganizationsHandler> logger) : ICommandHandler<ListOrganizationsCommand, ListOrganizationsResult>
{
    public async Task<ListOrganizationsResult> ExecuteAsync(ListOrganizationsCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all organizations with pagination page {PageNumber}, size {PageSize}", command.Pagination.PageNumber, command.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
        var response = await repository.GetAllAsync(command.Pagination);

        logger.LogInformation("Fetched {Count} organization records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListOrganizationsResult
        {
            Success = true,
            Message = "Organizations listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
    }

    private static PaginatedResult<OrganizationDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Organization?> result)
    {
        return new PaginatedResult<OrganizationDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}