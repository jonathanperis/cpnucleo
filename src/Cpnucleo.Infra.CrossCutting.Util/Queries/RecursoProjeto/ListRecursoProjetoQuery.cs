namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoProjeto;

public class ListRecursoProjetoQuery : IRequest<ListRecursoProjetoViewModel>
{
    public bool GetDependencies { get; set; }
}
