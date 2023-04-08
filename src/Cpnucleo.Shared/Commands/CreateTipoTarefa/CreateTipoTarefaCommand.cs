namespace Cpnucleo.Shared.Commands.CreateTipoTarefa;

public sealed record CreateTipoTarefaCommand(Guid Id, string Nome, string Image) : BaseCommand, IRequest<OperationResult>;