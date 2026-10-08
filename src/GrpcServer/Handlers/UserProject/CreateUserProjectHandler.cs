namespace GrpcServer.Handlers.UserProject;

// Dapper Repository Advanced
public sealed class CreateUserProjectHandler(IUnitOfWork unitOfWork, ILogger<CreateUserProjectHandler> logger) : ICommandHandler<CreateUserProjectCommand, CreateUserProjectResult>
{
    public async Task<CreateUserProjectResult> ExecuteAsync(CreateUserProjectCommand command, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogInformation("Checking if an userProject entity exists with Id: {UserProjectId}", command.Id);
            var repository = unitOfWork.GetRepository<Domain.Entities.UserProject>();
            var itemExists = await repository.ExistsAsync(command.Id, cancellationToken);

            if (itemExists)
            {
                logger.LogWarning("UserProject Id conflict for Id: {UserProjectId}", command.Id);
                return new CreateUserProjectResult
                {
                    Success = false,
                    Message = "this Id is already in use!"
                };
            }

            logger.LogInformation("Validation passed, proceeding to create new userProject entity.");
            var newItem = Domain.Entities.UserProject.Create(command.UserId, command.ProjectId, command.Id);
            logger.LogInformation("Created new userProject entity with Id: {UserProjectId}", newItem.Id);

            logger.LogInformation("Beginning transaction.");
            await unitOfWork.BeginTransactionAsync(cancellationToken);

            logger.LogInformation("Adding userProject to repository.");
            await repository.AddAsync(newItem, cancellationToken);

            logger.LogInformation("Committing transaction.");
            await unitOfWork.CommitAsync(cancellationToken);

            logger.LogInformation("Fetching userProject by Id: {UserProjectId}", newItem.Id);
            var createdItem = await repository.GetByIdAsync(newItem.Id, cancellationToken);

            var result = new CreateUserProjectResult
            {
                Success = true,
                Message = "UserProject created successfully.",
                UserProject = createdItem!.MapToDto()
            };

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