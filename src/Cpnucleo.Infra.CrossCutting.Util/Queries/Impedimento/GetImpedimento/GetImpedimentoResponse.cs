namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.GetImpedimento;

public class GetImpedimentoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public ImpedimentoViewModel Impedimento { get; set; }
}