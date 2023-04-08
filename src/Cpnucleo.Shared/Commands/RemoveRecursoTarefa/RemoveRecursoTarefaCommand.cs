namespace Cpnucleo.Shared.Commands.RemoveRecursoTarefa;

public sealed record RemoveRecursoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;