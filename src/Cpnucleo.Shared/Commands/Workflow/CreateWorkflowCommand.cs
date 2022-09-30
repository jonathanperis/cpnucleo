namespace Cpnucleo.Shared.Commands.Workflow;

public sealed record CreateWorkflowCommand(Guid Id, string Nome, int Ordem) : BaseCommand, IRequest<OperationResult>;