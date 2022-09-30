namespace Cpnucleo.Shared.Commands.Tarefa;

public sealed record RemoveTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;