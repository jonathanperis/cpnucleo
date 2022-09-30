namespace Cpnucleo.Shared.Commands.TipoTarefa;

public sealed record UpdateTipoTarefaCommand(Guid Id, string Nome, string Image) : BaseCommand, IRequest<OperationResult>;