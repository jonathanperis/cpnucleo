namespace Cpnucleo.Shared.Commands.TipoTarefa;

public sealed record RemoveTipoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;