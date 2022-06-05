namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class ListRecursoProjetoQuery : BaseQuery, IRequest<ListRecursoProjetoViewModel>
{
    public bool GetDependencies { get; set; }
}
