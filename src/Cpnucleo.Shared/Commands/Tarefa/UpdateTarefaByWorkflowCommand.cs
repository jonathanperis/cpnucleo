namespace Cpnucleo.Shared.Commands.Tarefa;

public sealed record UpdateTarefaByWorkflowCommand(Guid Id, Guid IdWorkflow) : BaseCommand, IRequest<OperationResult>;