namespace Cpnucleo.Shared.Commands.RemoveWorkflow;

public sealed record RemoveWorkflowCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;