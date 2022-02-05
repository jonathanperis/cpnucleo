namespace Cpnucleo.Application.Queries.RecursoProjeto.ListRecursoProjeto;

public class ListRecursoProjetoQuery : IRequest<ListRecursoProjetoViewModel>
{
    public bool GetDependencies { get; set; }
}
