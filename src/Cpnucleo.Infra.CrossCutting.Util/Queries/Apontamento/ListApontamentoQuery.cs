namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Apontamento;

public class ListApontamentoQuery : IRequest<ListApontamentoViewModel>
{
    public bool GetDependencies { get; set; }
}
