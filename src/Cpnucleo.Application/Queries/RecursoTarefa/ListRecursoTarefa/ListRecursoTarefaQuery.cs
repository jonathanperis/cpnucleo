namespace Cpnucleo.Application.Queries.RecursoTarefa.ListRecursoTarefa;

public class ListRecursoTarefaQuery : IRequest<ListRecursoTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
