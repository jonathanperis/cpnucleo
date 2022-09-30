namespace Cpnucleo.Shared.Commands.Workflow;

public sealed record RemoveWorkflowCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;