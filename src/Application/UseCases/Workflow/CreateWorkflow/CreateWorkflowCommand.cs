namespace Application.UseCases.Workflow.CreateWorkflow;

public sealed record CreateWorkflowCommand(string Name, int Order, Guid Id = default) : BaseCommand, IRequest<OperationResult>;
