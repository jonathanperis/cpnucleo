namespace Cpnucleo.Shared.Commands.RecursoTarefa;

public sealed record RemoveRecursoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;