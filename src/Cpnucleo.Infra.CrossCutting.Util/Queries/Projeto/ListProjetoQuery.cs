namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

public class ListProjetoQuery : BaseQuery, IRequest<ListProjetoViewModel>
{
    public bool GetDependencies { get; set; }
}
