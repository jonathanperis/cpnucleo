namespace Cpnucleo.Application.Queries.Tarefa.ListTarefa;

public class ListTarefaQuery : IRequest<ListTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
