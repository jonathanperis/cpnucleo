namespace GrpcServer.Handlers.AssignmentImpediment;

// Dapper Repository Advanced
public sealed class GetAssignmentImpedimentByIdHandler(IUnitOfWork unitOfWork, ILogger<GetAssignmentImpedimentByIdHandler> logger) : ICommandHandler<GetAssignmentImpedimentByIdCommand, GetAssignmentImpedimentByIdResult>
{
    public async Task<GetAssignmentImpedimentByIdResult> ExecuteAsync(GetAssignmentImpedimentByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching assignmentImpediment entity with Id: {AssignmentImpedimentId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.AssignmentImpediment>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("AssignmentImpediment not found with Id: {AssignmentImpedimentId}", command.Id);
            return new GetAssignmentImpedimentByIdResult();
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {AssignmentImpedimentId}", command.Id);
        var result = new GetAssignmentImpedimentByIdResult
        {
            AssignmentImpediment = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}