namespace Cpnucleo.Shared.Queries.GetImpedimentoTarefa;

public sealed record GetImpedimentoTarefaViewModel : BaseQuery
{
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
