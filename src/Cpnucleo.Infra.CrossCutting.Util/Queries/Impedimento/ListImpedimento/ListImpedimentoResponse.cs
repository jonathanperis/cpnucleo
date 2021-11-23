namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Impedimento.ListImpedimento;

public class ListImpedimentoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<ImpedimentoViewModel> Impedimentos { get; set; }
}