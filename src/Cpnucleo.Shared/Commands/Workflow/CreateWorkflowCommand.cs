namespace Cpnucleo.Shared.Commands.Workflow;

public record CreateWorkflowCommand(Guid Id, string Nome, int Ordem) : BaseCommand, IRequest<OperationResult>;