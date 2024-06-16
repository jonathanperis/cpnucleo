namespace Application.UseCases.Workflow.CreateWorkflow;

public sealed record CreateWorkflowCommand(string Name, byte Order, Ulid Id = default) : BaseCommand, IRequest<OperationResult>;
