namespace Cpnucleo.Shared.Queries.GetImpedimentoTarefa;

public sealed record GetImpedimentoTarefaViewModel : BaseQuery
{
    public ImpedimentoTarefaDto? ImpedimentoTarefa { get; set; }
    public required OperationResult OperationResult { get; set; }
}
