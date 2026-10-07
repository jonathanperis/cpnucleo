namespace GrpcServer.Handlers.Appointment;

// Dapper Repository Advanced
public sealed class ListAppointmentsHandler(IUnitOfWork unitOfWork, ILogger<ListAppointmentsHandler> logger) : ICommandHandler<ListAppointmentsCommand, ListAppointmentsResult>
{
    public async Task<ListAppointmentsResult> ExecuteAsync(ListAppointmentsCommand command, CancellationToken cancellationToken)
    {
        // Invalid or missing paging is InvalidArgument, the same rule REST applies.
        var pagination = PaginationParams.Require(command.Pagination);
        logger.LogInformation("Service started processing request.");
        logger.LogInformation("Fetching all appointments with pagination page {PageNumber}, size {PageSize}", pagination.PageNumber, pagination.PageSize);

        var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
        var response = await repository.GetAllAsync(pagination, cancellationToken);

        logger.LogInformation("Fetched {Count} appointment records", response.Data?.Count() ?? 0);
        logger.LogInformation("Mapping entities to DTOs.");

        var result = new ListAppointmentsResult
        {
            Success = true,
            Message = "Appointments listed successfully.",
            Result = MapToPaginatedDto(response)
        };

        logger.LogInformation("Mapping complete, setting response result.");
        logger.LogInformation("Service completed successfully.");

        return result;
    }

    private static PaginatedResult<AppointmentDto?> MapToPaginatedDto(PaginatedResult<Domain.Entities.Appointment?> result)
    {
        return new PaginatedResult<AppointmentDto?>
        {
            Data = result.Data?.Select(x => x?.MapToDto()).ToList(),
            TotalCount = result.TotalCount,
            PageNumber = result.PageNumber,
            PageSize = result.PageSize
        };
    }
}
