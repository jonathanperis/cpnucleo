namespace Cpnucleo.Shared.Commands.CreateWorkflow;

public sealed record CreateWorkflowCommand(string Nome, int Ordem) : BaseCommand, IRequest<OperationResult>;