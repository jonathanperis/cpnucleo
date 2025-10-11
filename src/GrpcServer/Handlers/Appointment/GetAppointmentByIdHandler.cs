namespace GrpcServer.Handlers.Appointment;

// Dapper Repository Advanced
public sealed class GetAppointmentByIdHandler(IUnitOfWork unitOfWork, ILogger<GetAppointmentByIdHandler> logger) : ICommandHandler<GetAppointmentByIdCommand, GetAppointmentByIdResult>
{
    public async Task<GetAppointmentByIdResult> ExecuteAsync(GetAppointmentByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching appointment entity with Id: {AppointmentId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("Appointment not found with Id: {AppointmentId}", command.Id);
            return new GetAppointmentByIdResult();
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {AppointmentId}", command.Id);
        var result = new GetAppointmentByIdResult
        {
            Appointment = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}