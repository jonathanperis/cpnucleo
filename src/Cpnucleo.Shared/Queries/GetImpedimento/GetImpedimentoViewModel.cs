namespace Cpnucleo.Shared.Queries.GetImpedimento;

public sealed record GetImpedimentoViewModel : BaseQuery
{
    public ImpedimentoDTO Impedimento { get; set; }
    public OperationResult OperationResult { get; set; }
}
