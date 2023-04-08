namespace Cpnucleo.Shared.Queries.ListImpedimento;

public sealed record ListImpedimentoViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoDTO> Impedimentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
