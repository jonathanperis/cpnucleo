namespace Cpnucleo.Shared.Commands.ImpedimentoTarefa;

public sealed record CreateImpedimentoTarefaCommand(Guid Id, string Descricao, Guid IdTarefa, Guid IdImpedimento) : BaseCommand, IRequest<OperationResult>;