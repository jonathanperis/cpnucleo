namespace Cpnucleo.Application.Queries.Projeto.ListProjeto;

public class ListProjetoQuery : IRequest<ListProjetoViewModel>
{
    public bool GetDependencies { get; }

    public ListProjetoQuery(bool getDependencies)
    {
        GetDependencies = getDependencies;
    }
}
