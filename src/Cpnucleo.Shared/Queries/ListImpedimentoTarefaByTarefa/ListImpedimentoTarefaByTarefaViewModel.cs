namespace Cpnucleo.Shared.Queries.ListImpedimentoTarefaByTarefa;

public sealed record ListImpedimentoTarefaByTarefaViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoTarefaDto> ImpedimentoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
