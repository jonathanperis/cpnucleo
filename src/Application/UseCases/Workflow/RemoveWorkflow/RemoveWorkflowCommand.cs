namespace Application.UseCases.Workflow.RemoveWorkflow;

public sealed record RemoveWorkflowCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;
