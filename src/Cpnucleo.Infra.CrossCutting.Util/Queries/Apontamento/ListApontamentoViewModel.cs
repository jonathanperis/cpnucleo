namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class ListApontamentoViewModel : BaseQuery
{
    public IEnumerable<ApontamentoDTO> Apontamentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
