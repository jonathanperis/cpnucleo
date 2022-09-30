namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public sealed record ListImpedimentoTarefaViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoTarefaDTO> ImpedimentoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
