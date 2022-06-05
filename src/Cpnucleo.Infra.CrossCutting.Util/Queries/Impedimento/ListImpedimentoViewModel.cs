namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class ListImpedimentoViewModel : BaseQuery
{
    public IEnumerable<ImpedimentoDTO> Impedimentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
