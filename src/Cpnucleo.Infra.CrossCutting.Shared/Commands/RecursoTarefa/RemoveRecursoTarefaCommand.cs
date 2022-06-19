namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.RecursoTarefa;

public record RemoveRecursoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;