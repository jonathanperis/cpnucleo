namespace GrpcServer.Handlers.AssignmentImpediment;

// Dapper Repository Advanced
public sealed class UpdateAssignmentImpedimentHandler(IUnitOfWork unitOfWork, ILogger<UpdateAssignmentImpedimentHandler> logger) : ICommandHandler<UpdateAssignmentImpedimentCommand, UpdateAssignmentImpedimentResult>
{
    public async Task<UpdateAssignmentImpedimentResult> ExecuteAsync(UpdateAssignmentImpedimentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an assignmentImpediment entity exists with Id: {AssignmentImpedimentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
            var item = await repository.GetByIdAsync(command.Id);

            if (item is null)
            {
                logger.LogWarning("AssignmentImpediment not found with Id: {AssignmentImpedimentId}", command.Id);
                return new UpdateAssignmentImpedimentResult 
                { 
                    Success = false,
                    Message = "AssignmentImpediment not found."
                };
            }

            logger.LogInformation("Updating assignmentImpediment entity with Id: {AssignmentImpedimentId}", command.Id);
            Domain.Entities.AssignmentImpediment.Update(item, command.Description, command.AssignmentId, command.ImpedimentId);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();
            
            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new UpdateAssignmentImpedimentResult 
            { 
                Success = success,
                Message = success ? "AssignmentImpediment updated successfully." : "Failed to update AssignmentImpediment."
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