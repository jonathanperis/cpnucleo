namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class CreateUserHandler(IUnitOfWork unitOfWork, ILogger<CreateUserHandler> logger, IPasswordHasher passwordHasher, IHttpContextAccessor context) : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> ExecuteAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        Common.Security.UserAdministration.RequireAdmin(context);
        logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {UserId}", command.Name, command.Id);

        try
        {
            logger.LogInformation("Checking if an user entity exists with Id: {UserId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.User>();
            var itemExists = await repository.ExistsAsync(command.Id, cancellationToken);

            if (itemExists)
            {
                logger.LogWarning("User Id conflict for Id: {UserId}", command.Id);
                return new CreateUserResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new user entity.");
            PasswordPolicy.Validate(command.Password);
            var passwordHash = passwordHasher.Hash(command.Password);
            var newItem = Domain.Entities.User.Create(command.Name, command.Login, passwordHash, command.Id);
            logger.LogInformation("Created new user entity with Id: {UserId}", newItem.Id);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Adding user to repository.");
            var createdId = await repository.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching user by Id: {UserId}", createdId);
            var createdItem = await repository.GetByIdAsync(createdId, cancellationToken);

            var result = new CreateUserResult
            {
                Success = true,
                Message = "User created successfully.",
                User = createdItem!.MapToDto()
            };

            logger.LogInformation("Service completed successfully.");

            return result;
        }
        catch
        {
            // Rejections and failures are translated (and unexpected ones logged) by the gRPC pipeline.
            await unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
