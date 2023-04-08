namespace Cpnucleo.Shared.Commands.UpdateRecursoTarefa;

public sealed record UpdateRecursoTarefaCommand(Guid Id, Guid IdRecurso, Guid IdTarefa) : BaseCommand, IRequest<OperationResult>;