namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa;

public class ListTarefaQuery : BaseQuery, IRequest<IEnumerable<TarefaViewModel>>
{
    public bool GetDependencies { get; set; }
}