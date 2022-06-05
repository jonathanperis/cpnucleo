namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class ListApontamentoQuery : BaseQuery, IRequest<ListApontamentoViewModel>
{
    public bool GetDependencies { get; set; }
}
