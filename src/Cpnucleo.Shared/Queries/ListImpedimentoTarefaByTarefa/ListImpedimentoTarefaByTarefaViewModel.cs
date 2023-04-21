namespace Cpnucleo.Shared.Queries.ListImpedimentoTarefaByTarefa;

public sealed record ListImpedimentoTarefaByTarefaViewModel : BaseQuery
{
    public List<ImpedimentoTarefaDto>? ImpedimentoTarefas { get; set; }
    public required OperationResult OperationResult { get; set; }
}
