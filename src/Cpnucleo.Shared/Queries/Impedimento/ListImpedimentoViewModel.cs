namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Impedimento;

public record ListImpedimentoViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoDTO> Impedimentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
