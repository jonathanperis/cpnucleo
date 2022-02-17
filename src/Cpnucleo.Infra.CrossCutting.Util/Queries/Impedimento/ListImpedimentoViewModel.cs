namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class ListImpedimentoViewModel
{
    public IEnumerable<ImpedimentoDTO> Impedimentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
