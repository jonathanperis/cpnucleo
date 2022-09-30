namespace Cpnucleo.Shared.Commands.RecursoTarefa;

public sealed record CreateRecursoTarefaCommand(Guid Id, Guid IdRecurso, Guid IdTarefa) : BaseCommand, IRequest<OperationResult>;