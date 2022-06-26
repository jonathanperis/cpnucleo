namespace Cpnucleo.Shared.Commands.Tarefa;

public record UpdateTarefaByWorkflowCommand(Guid Id, Guid IdWorkflow) : BaseCommand, IRequest<OperationResult>;