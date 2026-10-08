namespace GrpcServer.Handlers.Appointment;

// Dapper Repository Advanced
public sealed class RemoveAppointmentHandler(IUnitOfWork unitOfWork, ILogger<RemoveAppointmentHandler> logger) : ICommandHandler<RemoveAppointmentCommand, RemoveAppointmentResult>
{
    public async Task<RemoveAppointmentResult> ExecuteAsync(RemoveAppointmentCommand command, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} appointment entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.Appointment>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one appointment to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveAppointmentResult
                {
                    Success = false,
                    Message = "Appointment not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new RemoveAppointmentResult
            {
                Success = true,
                Message = "Appointment removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
