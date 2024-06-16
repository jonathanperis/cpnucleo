namespace Application.UseCases.Workflow.UpdateWorkflow;

public sealed record UpdateWorkflowCommand(Ulid Id, string Name, byte Order) : BaseCommand, IRequest<OperationResult>;
