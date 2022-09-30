namespace Cpnucleo.Shared.Queries.ImpedimentoTarefa;

public sealed record GetImpedimentoTarefaViewModel : BaseQuery
{
    public ImpedimentoTarefaDTO ImpedimentoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
