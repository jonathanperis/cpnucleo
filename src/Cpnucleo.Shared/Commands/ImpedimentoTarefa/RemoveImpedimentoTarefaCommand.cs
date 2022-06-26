namespace Cpnucleo.Shared.Commands.ImpedimentoTarefa;

public record RemoveImpedimentoTarefaCommand(Guid Id) : BaseCommand, IRequest<OperationResult>;