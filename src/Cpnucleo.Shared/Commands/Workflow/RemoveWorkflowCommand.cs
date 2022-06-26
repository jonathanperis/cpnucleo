namespace Cpnucleo.Shared.Commands.Workflow;

public record RemoveWorkflowCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;