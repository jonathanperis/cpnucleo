namespace GrpcServer.Handlers.UserProject;

// Dapper Repository Advanced
public sealed class UpdateUserProjectHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserProjectHandler> logger) : ICommandHandler<UpdateUserProjectCommand, UpdateUserProjectResult>
{
    public async Task<UpdateUserProjectResult> ExecuteAsync(UpdateUserProjectCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an userProject entity exists with Id: {UserProjectId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
            var item = await repository.GetByIdAsync(command.Id);

            if (item is null)
            {
                logger.LogWarning("UserProject not found with Id: {UserProjectId}", command.Id);
                return new UpdateUserProjectResult 
                { 
                    Success = false,
                    Message = "UserProject not found."
                };
            }

            logger.LogInformation("Updating userProject entity with Id: {UserProjectId}", command.Id);
            Domain.Entities.UserProject.Update(item, command.UserId, command.ProjectId);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync();
            
            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new UpdateUserProjectResult 
            { 
                Success = success,
                Message = success ? "UserProject updated successfully." : "Failed to update UserProject."
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