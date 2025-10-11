namespace GrpcServer.Handlers.Impediment;

// Dapper Repository Advanced
public sealed class ListImpedimentsHandler(IUnitOfWork unitOfWork, ILogger<ListImpedimentsHandler> logger) : ICommandHandler<ListImpedimentsCommand, ListImpedimentsResult>
{
    public async Task<ListImpedimentsResult> ExecuteAsync(ListImpedimentsCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all impediments with pagination page {PageNumber}, size {PageSize}", command.Pagination.PageNumber, command.Pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Impediment>();
        var response = await repository.GetAllAsync(command.Pagination);

        logger.LogInformation("Fetched {Count} impediment records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListImpedimentsResult
        {
            Success = true,
            Message = "Impediments listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
    }

    private static PaginatedResult<ImpedimentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Impediment?> result)
    {
        return new PaginatedResult<ImpedimentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}