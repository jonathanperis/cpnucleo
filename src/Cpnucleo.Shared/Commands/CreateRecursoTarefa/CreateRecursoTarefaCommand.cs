namespace Cpnucleo.Shared.Commands.CreateRecursoTarefa;

public sealed record CreateRecursoTarefaCommand(Guid Id, Guid IdRecurso, Guid IdTarefa) : BaseCommand, IRequest<OperationResult>;