namespace Cpnucleo.Shared.Queries.GetImpedimento;

public sealed record GetImpedimentoViewModel : BaseQuery
{
    public ImpedimentoDto? Impedimento { get; set; }
    public required OperationResult OperationResult { get; set; }
}
