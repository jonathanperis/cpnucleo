namespace GrpcServer.Handlers.UserAssignment;

// Dapper Repository Advanced
public sealed class UpdateUserAssignmentHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserAssignmentHandler> logger) : ICommandHandler<UpdateUserAssignmentCommand, UpdateUserAssignmentResult>
{
    public async Task<UpdateUserAssignmentResult> ExecuteAsync(UpdateUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an userAssignment entity exists with Id: {UserAssignmentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.UserAssignment>();
            var item = await repository.GetByIdAsync(command.Id);

            if (item is null)
            {
                logger.LogWarning("UserAssignment not found with Id: {UserAssignmentId}", command.Id);
                return new UpdateUserAssignmentResult 
                { 
                    Success = false,
                    Message = "UserAssignment not found."
                };
            }

            logger.LogInformation("Updating userAssignment entity with Id: {UserAssignmentId}", command.Id);
            Domain.Entities.UserAssignment.Update(item, command.UserId, command.AssignmentId);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();
            
            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new UpdateUserAssignmentResult 
            { 
                Success = success,
                Message = success ? "UserAssignment updated successfully." : "Failed to update UserAssignment."
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