namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class ListRecursoProjetoQuery : BaseQuery, IRequest<IEnumerable<RecursoProjetoViewModel>>
{
    public bool GetDependencies { get; set; }
}