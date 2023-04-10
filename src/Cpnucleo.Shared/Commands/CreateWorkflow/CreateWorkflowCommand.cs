namespace Cpnucleo.Shared.Commands.CreateWorkflow;

public sealed record CreateWorkflowCommand(string Nome, int Ordem, Guid Id = default) : BaseCommand, IRequest<OperationResult>;