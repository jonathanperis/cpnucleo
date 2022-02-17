namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class ListRecursoTarefaQuery : IRequest<ListRecursoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
