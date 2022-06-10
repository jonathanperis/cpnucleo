namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Apontamento;

public class ListApontamentoQuery : BaseQuery, IRequest<ListApontamentoViewModel>
{
    public bool GetDependencies { get; set; }
}
