namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class UpdateUserHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserHandler> logger) : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> ExecuteAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an user entity exists with Id: {UserId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.User>();
            var item = await repository.GetByIdAsync(command.Id);

            if (item is null)
            {
                logger.LogWarning("User not found with Id: {UserId}", command.Id);
                return new UpdateUserResult 
                { 
                    Success = false,
                    Message = "User not found."
                };
            }

            logger.LogInformation("Updating user entity with Id: {UserId}", command.Id);
            Domain.Entities.User.Update(item, command.Name, command.Password);

            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item);

            logger.LogInformation("Update result: {Success}", success);
            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Service completed successfully.");

            return new UpdateUserResult 
            { 
                Success = success,
                Message = success ? "User updated successfully." : "Failed to update User."
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