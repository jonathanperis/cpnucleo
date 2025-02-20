namespace Application.UseCases.Workflow.UpdateWorkflow;

public sealed record UpdateWorkflowCommand(Guid Id, string Name, int Order) : BaseCommand, IRequest<OperationResult>;
