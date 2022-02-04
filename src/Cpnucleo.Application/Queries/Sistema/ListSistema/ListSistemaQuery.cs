namespace Cpnucleo.Application.Queries.Sistema.ListSistema;

public class ListSistemaQuery : IRequest<ListSistemaViewModel>
{
    public bool GetDependencies { get; }

    public ListSistemaQuery(bool getDependencies)
    {
        GetDependencies = getDependencies;
    }
}
