namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class GetUserByIdHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdHandler> logger) : ICommandHandler<GetUserByIdCommand, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> ExecuteAsync(GetUserByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching user entity with Id: {UserId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.User>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("User not found with Id: {UserId}", command.Id);
            return new GetUserByIdResult();
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {UserId}", command.Id);
        var result = new GetUserByIdResult
        {
            User = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}