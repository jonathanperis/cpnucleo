namespace GrpcServer.Handlers.User;

// Dapper Repository Advanced
public sealed class GetUserByIdHandler(IUnitOfWork unitOfWork, ILogger<GetUserByIdHandler> logger) : ICommandHandler<GetUserByIdCommand, GetUserByIdResult>
{
    [Microsoft.AspNetCore.Authorization.Authorize(Policy = Common.Security.UserAdministration.Policy)]
    public async Task<GetUserByIdResult> ExecuteAsync(GetUserByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Fetching user entity with Id: {UserId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.User>();
        var item = await repository.GetByIdAsync(command.Id, cancellationToken);

        if (item is null)
        {
            logger.LogWarning("User not found with Id: {UserId}", command.Id);
            return new GetUserByIdResult
            {
                Success = false,
                Message = "User not found."
            };
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {UserId}", command.Id);
        var result = new GetUserByIdResult
        {
            Success = true,
            Message = "User fetched successfully.",
            User = item.MapToDto()
        };

        return result;
    }
}
