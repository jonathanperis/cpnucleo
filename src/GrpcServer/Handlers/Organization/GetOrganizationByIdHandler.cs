namespace GrpcServer.Handlers.Organization;

// Dapper Repository Advanced
public sealed class GetOrganizationByIdHandler(IUnitOfWork unitOfWork, ILogger<GetOrganizationByIdHandler> logger) : ICommandHandler<GetOrganizationByIdCommand, GetOrganizationByIdResult>
{
    public async Task<GetOrganizationByIdResult> ExecuteAsync(GetOrganizationByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching organization entity with Id: {OrganizationId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Organization>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("Organization not found with Id: {OrganizationId}", command.Id);
            return new GetOrganizationByIdResult();
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {OrganizationId}", command.Id);
        var result = new GetOrganizationByIdResult
        {
            Organization = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}