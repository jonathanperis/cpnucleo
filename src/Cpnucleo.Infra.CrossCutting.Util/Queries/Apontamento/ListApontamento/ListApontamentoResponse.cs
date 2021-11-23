namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.ListApontamento;

public class ListApontamentoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<ApontamentoViewModel> Apontamentos { get; set; }
}