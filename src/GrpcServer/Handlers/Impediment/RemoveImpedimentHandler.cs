namespace GrpcServer.Handlers.Impediment;

// Dapper Repository Advanced
public sealed class RemoveImpedimentHandler(IUnitOfWork unitOfWork, ILogger<RemoveImpedimentHandler> logger) : ICommandHandler<RemoveImpedimentCommand, RemoveImpedimentResult>
{
    public async Task<RemoveImpedimentResult> ExecuteAsync(RemoveImpedimentCommand command, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} impediment entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.Impediment>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one impediment to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveImpedimentResult
                {
                    Success = false,
                    Message = "Impediment not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new RemoveImpedimentResult
            {
                Success = true,
                Message = "Impediment removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
