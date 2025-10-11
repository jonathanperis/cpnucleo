namespace GrpcServer.Handlers.Appointment;

// EF Core
public sealed class RemoveAppointmentHandler(IApplicationDbContext dbContext, ILogger<RemoveAppointmentHandler> logger) : ICommandHandler<RemoveAppointmentCommand, RemoveAppointmentResult>
{
    public async Task<RemoveAppointmentResult> ExecuteAsync(RemoveAppointmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if appointment entities exist for Ids: {AppointmentIds}", string.Join(",", command.Ids));
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await dbContext.Appointments!.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
                if (item is null)
                {
                    logger.LogWarning("Appointment not found with Id: {AppointmentId}", id);
                    return new RemoveAppointmentResult { Success = false };
                }

                logger.LogInformation("Removing appointment entity with Id: {AppointmentId}", id);
                Domain.Entities.Appointment.Remove(item);

                logger.LogInformation("Updating repository for removed entity {AppointmentId}.", id);
                var result = await dbContext.SaveChangesAsync(cancellationToken);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed.");
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Service completed successfully.");

            return new RemoveAppointmentResult { Success = allSuccess };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}