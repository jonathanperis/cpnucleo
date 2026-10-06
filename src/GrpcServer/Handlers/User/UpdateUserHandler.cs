namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class UpdateUserHandler(IUnitOfWork unitOfWork, ILogger<UpdateUserHandler> logger, IPasswordHasher passwordHasher, IHttpContextAccessor context) : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> ExecuteAsync(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        Common.Security.UserAdministration.RequireAdmin(context);
        logger.LogInformation("Service started processing request.");

        try
        {
            logger.LogInformation("Checking if an user entity exists with Id: {UserId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.User>();
            var item = await repository.GetByIdAsync(command.Id, cancellationToken);

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
            // Same semantics as REST: a missing password or login keeps the current value.
            PasswordHash? passwordHash = null;
            if (!string.IsNullOrWhiteSpace(command.Password))
            {
                PasswordPolicy.Validate(command.Password);
                passwordHash = passwordHasher.Hash(command.Password);
            }

            Domain.Entities.User.Update(item, command.Name, command.Login, passwordHash);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);
            
            logger.LogInformation("Updating entity in repository.");
            var success = await repository.UpdateAsync(item, cancellationToken);

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
        catch
        {
            // Rejections and failures are translated (and unexpected ones logged) by the gRPC pipeline.
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
