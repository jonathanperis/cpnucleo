namespace Cpnucleo.Shared.Queries.ListImpedimentoTarefa;

public sealed record ListImpedimentoTarefaViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoTarefaDto> ImpedimentoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
