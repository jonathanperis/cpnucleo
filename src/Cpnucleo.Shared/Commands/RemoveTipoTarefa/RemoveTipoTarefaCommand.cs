namespace Cpnucleo.Shared.Commands.RemoveTipoTarefa;

public sealed record RemoveTipoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;