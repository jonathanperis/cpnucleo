namespace Cpnucleo.Shared.Queries.ListImpedimentoTarefa;

public sealed record ListImpedimentoTarefaViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoTarefaDTO> ImpedimentoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
