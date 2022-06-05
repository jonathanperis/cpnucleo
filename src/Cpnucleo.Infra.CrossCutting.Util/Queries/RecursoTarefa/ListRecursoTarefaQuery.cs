namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class ListRecursoTarefaQuery : BaseQuery, IRequest<ListRecursoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
