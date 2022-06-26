namespace Cpnucleo.Shared.Commands.ImpedimentoTarefa;

public record UpdateImpedimentoTarefaCommand(Guid Id, string Descricao, Guid IdTarefa, Guid IdImpedimento) : BaseCommand, IRequest<OperationResult>;