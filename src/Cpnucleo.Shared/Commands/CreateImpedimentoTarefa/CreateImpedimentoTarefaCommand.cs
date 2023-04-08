namespace Cpnucleo.Shared.Commands.CreateImpedimentoTarefa;

public sealed record CreateImpedimentoTarefaCommand(Guid Id, string Descricao, Guid IdTarefa, Guid IdImpedimento) : BaseCommand, IRequest<OperationResult>;