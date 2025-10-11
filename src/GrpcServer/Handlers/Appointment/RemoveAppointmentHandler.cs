namespace GrpcServer.Handlers.Appointment;

// Dapper Repository Advanced
public sealed class RemoveAppointmentHandler(IUnitOfWork unitOfWork, ILogger<RemoveAppointmentHandler> logger) : ICommandHandler<RemoveAppointmentCommand, RemoveAppointmentResult>
{
    public async Task<RemoveAppointmentResult> ExecuteAsync(RemoveAppointmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();

            logger.LogInformation("Checking if appointment entities exist for Ids: {AppointmentIds}", string.Join(",", command.Ids));
            var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
            var allSuccess = true;

            foreach (var id in command.Ids)
            {
                var item = await repository.GetByIdAsync(id);
                if (item is null)
                {
                    logger.LogWarning("Appointment not found with Id: {AppointmentId}", id);
                    await unitOfWork.RollbackAsync(cancellationToken);
                    return new RemoveAppointmentResult 
                    { 
                        Success = false,
                        Message = "Appointment not found."
                    };
                }

                logger.LogInformation("Removing appointment entity with Id: {AppointmentId}", id);
                Domain.Entities.Appointment.Remove(item);

                logger.LogInformation("Deleting appointment entity from repository with Id: {AppointmentId}.", id);
                var result = await repository.DeleteAsync(id);

                if (!result) allSuccess = false;
            }

            if (!allSuccess)
            {
                logger.LogWarning("One or more deletions failed, rolling back transaction.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveAppointmentResult 
                { 
                    Success = false,
                    Message = "One or more appointments could not be deleted."
                };
            }

            logger.LogInformation("Remove result: {Success}", allSuccess);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new RemoveAppointmentResult 
            { 
                Success = allSuccess,
                Message = allSuccess ? "Appointment removed successfully." : "Failed to remove Appointment."
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command. Rolling back transaction.");
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}