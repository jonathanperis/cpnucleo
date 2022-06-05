namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class ListTarefaQuery : BaseQuery, IRequest<ListTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
