namespace Cpnucleo.Shared.Queries.Tarefa;

public sealed record ListTarefaViewModel : BaseQuery
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
