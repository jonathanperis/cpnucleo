namespace Cpnucleo.Shared.Commands.CreateRecursoTarefa;

public sealed record CreateRecursoTarefaCommand(Guid IdRecurso, Guid IdTarefa, Guid Id = default) : BaseCommand, IRequest<OperationResult>;