namespace Cpnucleo.Shared.Commands.Workflow;

public sealed record UpdateWorkflowCommand(Guid Id, string Nome, int Ordem) : BaseCommand, IRequest<OperationResult>;