namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento;

public class GetImpedimentoViewModel : BaseQuery
{
    public ImpedimentoDTO Impedimento { get; set; }
    public OperationResult OperationResult { get; set; }
}
