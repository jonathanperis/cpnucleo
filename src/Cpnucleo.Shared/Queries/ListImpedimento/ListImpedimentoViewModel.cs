namespace Cpnucleo.Shared.Queries.ListImpedimento;

public sealed record ListImpedimentoViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoDto> Impedimentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
