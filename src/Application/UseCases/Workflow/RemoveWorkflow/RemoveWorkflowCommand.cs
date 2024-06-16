namespace Application.UseCases.Workflow.RemoveWorkflow;

public sealed record RemoveWorkflowCommand(Ulid Id) : BaseCommand, IRequest<OperationResult>;
