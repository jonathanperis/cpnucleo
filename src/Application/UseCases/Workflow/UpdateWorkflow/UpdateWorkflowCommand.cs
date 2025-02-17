namespace Application.UseCases.Workflow.UpdateWorkflow;

public sealed record UpdateWorkflowCommand(Guid Id, string Name, byte Order) : BaseCommand, IRequest<OperationResult>;
