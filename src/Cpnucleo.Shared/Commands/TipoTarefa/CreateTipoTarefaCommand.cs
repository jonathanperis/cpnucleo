namespace Cpnucleo.Shared.Commands.TipoTarefa;

public sealed record CreateTipoTarefaCommand(Guid Id, string Nome, string Image) : BaseCommand, IRequest<OperationResult>;