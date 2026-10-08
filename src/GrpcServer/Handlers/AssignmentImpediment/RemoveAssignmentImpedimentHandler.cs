namespace GrpcServer.Handlers.AssignmentImpediment;

// Dapper Repository Advanced
public sealed class RemoveAssignmentImpedimentHandler(IUnitOfWork unitOfWork, ILogger<RemoveAssignmentImpedimentHandler> logger) : ICommandHandler<RemoveAssignmentImpedimentCommand, RemoveAssignmentImpedimentResult>
{
    public async Task<RemoveAssignmentImpedimentResult> ExecuteAsync(RemoveAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        var ids = BatchIds.Normalize(command.Ids);

        try
        {
            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Removing {Count} assignment impediment entities atomically.", ids.Length);
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
            var success = await repository.RemoveManyAsync(ids, cancellationToken);

            if (!success)
            {
                logger.LogWarning("At least one assignment impediment to remove was not found; rolling back.");
                await unitOfWork.RollbackAsync(cancellationToken);
                return new RemoveAssignmentImpedimentResult
                {
                    Success = false,
                    Message = "AssignmentImpediment not found."
                };
            }

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new RemoveAssignmentImpedimentResult
            {
                Success = true,
                Message = "AssignmentImpediment removed successfully."
            };
        }
        catch
        {
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
