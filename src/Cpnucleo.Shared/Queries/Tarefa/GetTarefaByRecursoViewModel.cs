namespace Cpnucleo.Shared.Queries.Tarefa;

public sealed record GetTarefaByRecursoViewModel : BaseQuery
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
