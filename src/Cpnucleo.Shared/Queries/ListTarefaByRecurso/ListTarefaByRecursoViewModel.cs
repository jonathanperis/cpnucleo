namespace Cpnucleo.Shared.Queries.ListTarefaByRecurso;

public sealed record ListTarefaByRecursoViewModel : BaseQuery
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
