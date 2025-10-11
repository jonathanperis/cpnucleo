namespace GrpcServer.Handlers.Impediment;

// Dapper Repository Advanced
public sealed class GetImpedimentByIdHandler(IUnitOfWork unitOfWork, ILogger<GetImpedimentByIdHandler> logger) : ICommandHandler<GetImpedimentByIdCommand, GetImpedimentByIdResult>
{
    public async Task<GetImpedimentByIdResult> ExecuteAsync(GetImpedimentByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching impediment entity with Id: {ImpedimentId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Impediment>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("Impediment not found with Id: {ImpedimentId}", command.Id);
            return new GetImpedimentByIdResult();
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {ImpedimentId}", command.Id);
        var result = new GetImpedimentByIdResult
        {
            Impediment = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}