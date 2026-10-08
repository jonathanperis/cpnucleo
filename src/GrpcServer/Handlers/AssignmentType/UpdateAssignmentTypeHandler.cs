namespace GrpcServer.Handlers.AssignmentType;

// Dapper Repository Advanced
public sealed class UpdateAssignmentTypeHandler(IUnitOfWork unitOfWork, ILogger<UpdateAssignmentTypeHandler> logger) : ICommandHandler<UpdateAssignmentTypeCommand, UpdateAssignmentTypeResult>
{
    public async Task<UpdateAssignmentTypeResult> ExecuteAsync(UpdateAssignmentTypeCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if an assignmentType entity exists with Id: {AssignmentTypeId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentType>();
            var item = await repository.GetByIdAsync(command.Id, cancellationToken);

            if (item is null)
            {
                logger.LogWarning("AssignmentType not found with Id: {AssignmentTypeId}", command.Id);
                return new UpdateAssignmentTypeResult 
                { 
                    Success = false,
                    Message = "AssignmentType not found."
                };
            }

            logger.LogInformation("Updating assignmentType entity with Id: {AssignmentTypeId}", command.Id);
            Domain.Entities.AssignmentType.Update(item, command.Name);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            
            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item, cancellationToken);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            return new UpdateAssignmentTypeResult 
            { 
                Success = success,
                Message = success ? "AssignmentType updated successfully." : "Failed to update AssignmentType."
            };
        }
        catch
        {
            // Rejections and failures are translated (and unexpected ones logged) by the gRPC pipeline.
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}