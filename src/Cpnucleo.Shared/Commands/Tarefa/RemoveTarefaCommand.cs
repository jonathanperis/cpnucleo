namespace Cpnucleo.Infra.CrossCutting.Shared.Commands.Tarefa;

public record RemoveTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;