namespace Cpnucleo.Application.Queries.Tarefa.ListTarefa;

public class ListTarefaQuery : IRequest<ListTarefaViewModel>
{
    public bool GetDependencies { get; }

    public ListTarefaQuery(bool getDependencies)
    {
        GetDependencies = getDependencies;
    }
}
