namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

public class ListRecursoQuery : IRequest<ListRecursoViewModel>
{
    public bool GetDependencies { get; set; }
}
