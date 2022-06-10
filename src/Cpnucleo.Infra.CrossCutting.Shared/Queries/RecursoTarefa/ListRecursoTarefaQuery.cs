namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public class ListRecursoTarefaQuery : BaseQuery, IRequest<ListRecursoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
