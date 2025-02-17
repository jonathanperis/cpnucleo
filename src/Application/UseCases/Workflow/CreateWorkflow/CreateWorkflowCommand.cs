namespace Application.UseCases.Workflow.CreateWorkflow;

public sealed record CreateWorkflowCommand(string Name, byte Order, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
