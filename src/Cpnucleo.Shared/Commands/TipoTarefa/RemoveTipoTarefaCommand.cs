namespace Cpnucleo.Shared.Commands.TipoTarefa;

public record RemoveTipoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;