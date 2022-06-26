namespace Cpnucleo.Shared.Commands.TipoTarefa;

public record CreateTipoTarefaCommand(Guid Id, string Nome, string Image) : BaseCommand, IRequest<OperationResult>;