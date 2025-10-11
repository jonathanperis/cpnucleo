namespace GrpcServer.Handlers.Appointment;

// Dapper Repository Advanced
public sealed class CreateAppointmentHandler(IUnitOfWork unitOfWork, ILogger<CreateAppointmentHandler> logger) : ICommandHandler<CreateAppointmentCommand, CreateAppointmentResult>
{
    public async Task<CreateAppointmentResult> ExecuteAsync(CreateAppointmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {AppointmentId}", command.Name, command.Id);

            logger.LogInformation("Checking if an appointment entity exists with Id: {AppointmentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
            var itemExists = await repository.ExistsAsync(command.Id);

            if (itemExists)
            {
                logger.LogWarning("Appointment Id conflict for Id: {AppointmentId}", command.Id);
                return new CreateAppointmentResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new appointment entity.");
            var newItem = Domain.Entities.Appointment.Create(command.Description,
                                                             command.KeepDate,
                                                             command.AmountHours,
                                                             command.AssignmentId,
                                                             command.UserId,
                                                             command.Id);
            logger.LogInformation("Created new appointment entity with Id: {AppointmentId}", newItem.Id);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Adding appointment to repository.");
            var createdId = await repository.AddAsync(newItem);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching appointment by Id: {AppointmentId}", newItem.Id);
            var createdItem = await repository.GetByIdAsync(newItem.Id);

            var result = new CreateAppointmentResult
            {
                Success = true,
                Message = "Appointment created successfully.",
                Appointment = createdItem!.MapToDto()
            };
            
            logger.LogInformation("Service completed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}