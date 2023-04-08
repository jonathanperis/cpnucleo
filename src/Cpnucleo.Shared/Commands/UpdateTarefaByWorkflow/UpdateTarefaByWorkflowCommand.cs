namespace Cpnucleo.Shared.Commands.UpdateTarefaByWorkflow;

public sealed record UpdateTarefaByWorkflowCommand(Guid Id, Guid IdWorkflow) : BaseCommand, IRequest<OperationResult>;