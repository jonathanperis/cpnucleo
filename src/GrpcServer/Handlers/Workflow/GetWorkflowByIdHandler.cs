namespace GrpcServer.Handlers.Workflow;

// Dapper Repository Advanced
public sealed class GetWorkflowByIdHandler(IUnitOfWork unitOfWork, ILogger<GetWorkflowByIdHandler> logger) : ICommandHandler<GetWorkflowByIdCommand, GetWorkflowByIdResult>
{
    public async Task<GetWorkflowByIdResult> ExecuteAsync(GetWorkflowByIdCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Service started processing request.");

        logger.LogInformation("Fetching workflow entity with Id: {WorkflowId}", command.Id);
        var repository = unitOfWork.GetRepository<Domain.Entities.Workflow>();
        var item = await repository.GetByIdAsync(command.Id);

        if (item is null)
        {
            logger.LogWarning("Workflow not found with Id: {WorkflowId}", command.Id);
            return new GetWorkflowByIdResult();
        }

        logger.LogInformation("Mapping entity to DTO and setting response for Id: {WorkflowId}", command.Id);
        var result = new GetWorkflowByIdResult
        {
            Workflow = item.MapToDto()
        };

        logger.LogInformation("Service completed successfully.");

        return result;
    }
}