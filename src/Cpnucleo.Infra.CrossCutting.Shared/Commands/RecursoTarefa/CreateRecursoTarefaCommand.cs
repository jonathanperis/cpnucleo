namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoTarefa;

public record CreateRecursoTarefaCommand(Guid Id, Guid IdRecurso, Guid IdTarefa) : BaseCommand, IRequest<OperationResult>;