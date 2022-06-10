namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoProjeto;

public class ListRecursoProjetoQuery : BaseQuery, IRequest<ListRecursoProjetoViewModel>
{
    public bool GetDependencies { get; set; }
}
