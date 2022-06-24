namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.ImpedimentoTarefa;

public record ListImpedimentoTarefaViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoTarefaDTO> ImpedimentoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
