namespace GrpcServer.Handlers.Appointment;

// EF Core
public sealed class CreateAppointmentHandler(IApplicationDbContext dbContext, ILogger<CreateAppointmentHandler> logger) : ICommandHandler<CreateAppointmentCommand, CreateAppointmentResult>
{
    public async Task<CreateAppointmentResult> ExecuteAsync(CreateAppointmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {AppointmentId}", command.Name, command.Id);

            logger.LogInformation("Checking if an appointment entity exists with Id: {AppointmentId}", command.Id);
            var itemExists = dbContext.Appointments!.Any(x => x.Id == command.Id);

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

            logger.LogInformation("Adding appointment to repository.");
            await dbContext.Appointments!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching appointment by Id: {AppointmentId}", newItem.Id);
            var createdItem = await dbContext.Appointments!.FindAsync([newItem.Id], cancellationToken: cancellationToken);

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
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}