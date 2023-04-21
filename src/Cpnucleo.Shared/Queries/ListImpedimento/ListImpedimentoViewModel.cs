namespace Cpnucleo.Shared.Queries.ListImpedimento;

public sealed record ListImpedimentoViewModel : BaseQuery
{
    public List<ImpedimentoDto>? Impedimentos { get; set; }
    public required OperationResult OperationResult { get; set; }
}
