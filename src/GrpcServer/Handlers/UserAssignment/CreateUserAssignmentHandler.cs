namespace GrpcServer.Handlers.UserAssignment;

// EF Core
public sealed class CreateUserAssignmentHandler(IApplicationDbContext dbContext, ILogger<CreateUserAssignmentHandler> logger) : ICommandHandler<CreateUserAssignmentCommand, CreateUserAssignmentResult>
{
    public async Task<CreateUserAssignmentResult> ExecuteAsync(CreateUserAssignmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload UserId: {UserId}, UserAssignmentId: {UserAssignmentId}, Id: {Id}", command.UserId, command.AssignmentId, command.Id);

            logger.LogInformation("Checking if an userAssignment entity exists with Id: {UserAssignmentId}", command.Id);
            var itemExists = dbContext.UserAssignments!.Any(x => x.Id == command.Id);

            if (itemExists)
            {
                logger.LogWarning("UserAssignment Id conflict for Id: {UserAssignmentId}", command.Id);
                return new CreateUserAssignmentResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new userAssignment entity.");
            var newItem = Domain.Entities.UserAssignment.Create(command.UserId, command.AssignmentId, command.Id);
            logger.LogInformation("Created new userAssignment entity with Id: {UserAssignmentId}", newItem.Id);

            logger.LogInformation("Adding userAssignment to repository.");
            await dbContext.UserAssignments!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching userAssignment by Id: {UserAssignmentId}", newItem.Id);
            var createdItem = await dbContext.UserAssignments!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

            var result = new CreateUserAssignmentResult
            {
                Success = true,
                Message = "UserAssignment created successfully.",
                UserAssignment = createdItem!.MapToDto()
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