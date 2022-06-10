namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Projeto;

public class ListProjetoQuery : BaseQuery, IRequest<ListProjetoViewModel>
{
    public bool GetDependencies { get; set; }
}
