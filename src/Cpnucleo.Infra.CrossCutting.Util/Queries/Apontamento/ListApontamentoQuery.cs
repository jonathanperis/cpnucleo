namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class ListApontamentoQuery : BaseQuery, IRequest<IEnumerable<ApontamentoViewModel>>
{
    public bool GetDependencies { get; set; }
}