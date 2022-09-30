namespace Cpnucleo.Shared.Commands.RecursoTarefa;

public sealed record UpdateRecursoTarefaCommand(Guid Id, Guid IdRecurso, Guid IdTarefa) : BaseCommand, IRequest<OperationResult>;