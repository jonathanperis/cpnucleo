namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

public class ListProjetoQuery : IRequest<ListProjetoViewModel>
{
    public bool GetDependencies { get; set; }
}
