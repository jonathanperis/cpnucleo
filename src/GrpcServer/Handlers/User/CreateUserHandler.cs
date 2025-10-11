namespace GrpcServer.Handlers.User;

// EF Core
public sealed class CreateUserHandler(IApplicationDbContext dbContext, ILogger<CreateUserHandler> logger) : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> ExecuteAsync(CreateUserCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {UserId}", command.Name, command.Id);

            logger.LogInformation("Checking if an user entity exists with Id: {UserId}", command.Id);
            var itemExists = dbContext.Users!.Any(x => x.Id == command.Id);

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
            var newItem = Domain.Entities.User.Create(command.Name, command.Login, command.Password, command.Id);
            logger.LogInformation("Created new user entity with Id: {UserId}", newItem.Id);

            logger.LogInformation("Adding user to repository.");
            await dbContext.Users!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching user by Id: {UserId}", newItem.Id);
            var createdItem = await dbContext.Users!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

            var result = new CreateUserResult
            {
                Success = true,
                Message = "User created successfully.",
                User = createdItem!.MapToDto()
            };

            logger.LogInformation("Service completed successfully.");

            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while processing the command.");
            throw;
        }
    }
}