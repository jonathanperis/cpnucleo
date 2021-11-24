namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class ListRecursoTarefaQuery : BaseQuery, IRequest<IEnumerable<RecursoTarefaViewModel>>
{
    public bool GetDependencies { get; set; }
}