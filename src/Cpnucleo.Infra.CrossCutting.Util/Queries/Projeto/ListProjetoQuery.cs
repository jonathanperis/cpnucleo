namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

public class ListProjetoQuery : BaseQuery, IRequest<IEnumerable<ProjetoViewModel>>
{
    public bool GetDependencies { get; set; }
}