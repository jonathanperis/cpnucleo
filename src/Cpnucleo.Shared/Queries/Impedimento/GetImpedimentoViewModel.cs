namespace Cpnucleo.Shared.Queries.Impedimento;

public record GetImpedimentoViewModel : BaseQuery
{
    public ImpedimentoDTO Impedimento { get; set; }
    public OperationResult OperationResult { get; set; }
}
