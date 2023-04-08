namespace Cpnucleo.Shared.Commands.UpdateImpedimentoTarefa;

public sealed record UpdateImpedimentoTarefaCommand(Guid Id, string Descricao, Guid IdTarefa, Guid IdImpedimento) : BaseCommand, IRequest<OperationResult>;