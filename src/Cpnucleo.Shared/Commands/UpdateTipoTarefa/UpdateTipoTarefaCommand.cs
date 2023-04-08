namespace Cpnucleo.Shared.Commands.UpdateTipoTarefa;

public sealed record UpdateTipoTarefaCommand(Guid Id, string Nome, string Image) : BaseCommand, IRequest<OperationResult>;