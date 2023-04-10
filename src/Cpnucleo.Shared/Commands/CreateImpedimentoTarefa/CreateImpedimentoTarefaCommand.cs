namespace Cpnucleo.Shared.Commands.CreateImpedimentoTarefa;

public sealed record CreateImpedimentoTarefaCommand(string Descricao, Guid IdTarefa, Guid IdImpedimento, Guid Id = default) : BaseCommand, IRequest<OperationResult>;