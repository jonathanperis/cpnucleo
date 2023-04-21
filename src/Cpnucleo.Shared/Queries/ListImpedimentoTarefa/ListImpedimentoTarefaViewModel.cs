namespace Cpnucleo.Shared.Queries.ListImpedimentoTarefa;

public sealed record ListImpedimentoTarefaViewModel : BaseQuery
{
    public List<ImpedimentoTarefaDto>? ImpedimentoTarefas { get; set; }
    public required OperationResult OperationResult { get; set; }
}
