namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

public class ListRecursoQuery : BaseQuery, IRequest<ListRecursoViewModel>
{
    public bool GetDependencies { get; set; }
}
