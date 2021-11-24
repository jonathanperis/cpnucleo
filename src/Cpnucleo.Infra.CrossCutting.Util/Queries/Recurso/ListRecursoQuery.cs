namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

public class ListRecursoQuery : BaseQuery, IRequest<IEnumerable<RecursoViewModel>>
{
    public bool GetDependencies { get; set; }
}