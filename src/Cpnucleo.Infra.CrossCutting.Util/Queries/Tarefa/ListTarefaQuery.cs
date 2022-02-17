namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class ListTarefaQuery : IRequest<ListTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
