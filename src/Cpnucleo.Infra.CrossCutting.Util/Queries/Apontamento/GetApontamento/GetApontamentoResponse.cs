namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento.GetApontamento;

public class GetApontamentoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public ApontamentoViewModel Apontamento { get; set; }
}