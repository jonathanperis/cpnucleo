namespace Cpnucleo.Shared.Commands.RemoveImpedimentoTarefa;

public sealed record RemoveImpedimentoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;