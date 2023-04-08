namespace Cpnucleo.Shared.Commands.UpdateWorkflow;

public sealed record UpdateWorkflowCommand(Guid Id, string Nome, int Ordem) : BaseCommand, IRequest<OperationResult>;