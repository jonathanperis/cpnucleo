namespace Cpnucleo.Shared.Commands.ImpedimentoTarefa;

public sealed record RemoveImpedimentoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;