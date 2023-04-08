namespace Cpnucleo.Shared.Commands.CreateRecursoTarefa;

public sealed record CreateRecursoTarefaCommand(Guid IdRecurso, Guid IdTarefa) : BaseCommand, IRequest<OperationResult>;