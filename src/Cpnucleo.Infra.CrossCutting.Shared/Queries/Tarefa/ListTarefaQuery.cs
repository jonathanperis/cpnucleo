namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public class ListTarefaQuery : BaseQuery, IRequest<ListTarefaViewModel>
{
    public bool GetDependencies { get; set; }
}
