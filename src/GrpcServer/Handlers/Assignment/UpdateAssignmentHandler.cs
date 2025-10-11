namespace GrpcServer.Handlers.Assignment;

// Dapper Repository Advanced
public sealed class UpdateAssignmentHandler(IUnitOfWork unitOfWork, ILogger<UpdateAssignmentHandler> logger) : ICommandHandler<UpdateAssignmentCommand, UpdateAssignmentResult>
{
    public async Task<UpdateAssignmentResult> ExecuteAsync(UpdateAssignmentCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an assignment entity exists with Id: {AssignmentId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.Assignment>();
            var item = await repository.GetByIdAsync(command.Id);

            if (item is null)
            {
                logger.LogWarning("Assignment not found with Id: {AssignmentId}", command.Id);
                return new UpdateAssignmentResult 
                { 
                    Success = false,
                    Message = "Assignment not found."
                };
            }

            logger.LogInformation("Updating assignment entity with Id: {AssignmentId}", command.Id);
            Domain.Entities.Assignment.Update(item,
                                             command.Name,
                                             command.Description,
                                             command.StartDate,
                                             command.EndDate,
                                             command.AmountHours,
                                             command.ProjectId,
                                             command.WorkflowId,
                                             command.UserId,
                                             command.AssignmentTypeId);

            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new UpdateAssignmentResult 
            { 
                Success = success,
                Message = success ? "Assignment updated successfully." : "Failed to update Assignment."
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