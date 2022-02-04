namespace Cpnucleo.Application.Queries.RecursoTarefa.ListRecursoTarefa;

public class ListRecursoTarefaQuery : IRequest<ListRecursoTarefaViewModel>
{
    public bool GetDependencies { get; }

    public ListRecursoTarefaQuery(bool getDependencies)
    {
        GetDependencies = getDependencies;
    }
}
