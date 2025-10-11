namespace GrpcServer.Handlers.Assignment;

// EF Core
public sealed class CreateAssignmentHandler(IApplicationDbContext dbContext, ILogger<CreateAssignmentHandler> logger) : ICommandHandler<CreateAssignmentCommand, CreateAssignmentResult>
{
    public async Task<CreateAssignmentResult> ExecuteAsync(CreateAssignmentCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Service started processing request with payload Name: {Name}, Id: {AssignmentId}", command.Name, command.Id);

            logger.LogInformation("Checking if an assignment entity exists with Id: {AssignmentId}", command.Id);
            var itemExists = dbContext.Assignments!.Any(x => x.Id == command.Id);

            if (itemExists)
            {
                logger.LogWarning("Assignment Id conflict for Id: {AssignmentId}", command.Id);
                return new CreateAssignmentResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new assignment entity.");
            var newItem = Domain.Entities.Assignment.Create(command.Name,
                                                            command.Description,
                                                            command.StartDate,
                                                            command.EndDate,
                                                            command.AmountHours,
                                                            command.ProjectId,
                                                            command.WorkflowId,
                                                            command.UserId,
                                                            command.AssignmentTypeId,
                                                            command.Id);
            logger.LogInformation("Created new assignment entity with Id: {AssignmentId}", newItem.Id);

            logger.LogInformation("Adding assignment to repository.");
            await dbContext.Assignments!.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await dbContext.SaveChangesAsync(cancellationToken);

            logger.LogInformation("Fetching assignment by Id: {AssignmentId}", newItem.Id);
            var createdItem = await dbContext.Assignments!.FindAsync([newItem.Id, cancellationToken], cancellationToken: cancellationToken);

            var result = new CreateAssignmentResult
            {
                Success = true,
                Message = "Assignment created successfully.",
                Assignment = createdItem!.MapToDto()
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