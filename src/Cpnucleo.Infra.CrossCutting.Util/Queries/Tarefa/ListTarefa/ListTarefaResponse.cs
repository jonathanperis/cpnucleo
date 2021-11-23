namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.ListTarefa;

public class ListTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<TarefaViewModel> Tarefas { get; set; }
}