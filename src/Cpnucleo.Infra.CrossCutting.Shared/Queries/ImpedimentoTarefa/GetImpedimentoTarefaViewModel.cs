namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.ImpedimentoTarefa;

public record GetImpedimentoTarefaViewModel : BaseQuery
{
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
