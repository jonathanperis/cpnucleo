namespace Cpnucleo.Application.Queries.Recurso.ListRecurso;

public class ListRecursoQuery : IRequest<ListRecursoViewModel>
{
    public bool GetDependencies { get; set; }
}
