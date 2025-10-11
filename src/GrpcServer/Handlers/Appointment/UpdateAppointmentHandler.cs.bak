namespace GrpcServer.Handlers.Appointment;

// EF Core
public sealed class UpdateAppointmentHandler(IApplicationDbContext dbContext, ILogger<UpdateAppointmentHandler> logger) : ICommandHandler<UpdateAppointmentCommand, UpdateAppointmentResult>
{
    public async Task<UpdateAppointmentResult> ExecuteAsync(UpdateAppointmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an appointment entity exists with Id: {AppointmentId}", command.Id);
            var item = await dbContext.Appointments!.FindAsync([command.Id, cancellationToken], cancellationToken: cancellationToken);

            if (item is null)
            {
                logger.LogWarning("Appointment not found with Id: {AppointmentId}", command.Id);
                return new UpdateAppointmentResult 
                { 
                    Success = false,
                    Message = "Appointment not found."
                };
            }

            logger.LogInformation("Updating appointment entity with Id: {AppointmentId}", command.Id);
            Domain.Entities.Appointment.Update(item,
                                               command.Description,
                                               command.KeepDate,
                                               command.AmountHours,
                                               command.AssignmentId,
                                               command.UserId);

            logger.LogInformation("Updating entity in repository.");
            var success = await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Service completed successfully.");

            return new UpdateAppointmentResult 
            { 
                Success = success,
                Message = success ? "Appointment updated successfully." : "Failed to update Appointment."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}