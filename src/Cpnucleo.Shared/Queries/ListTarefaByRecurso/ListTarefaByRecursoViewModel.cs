namespace Cpnucleo.Shared.Queries.ListTarefaByRecurso;

public sealed record ListTarefaByRecursoViewModel : BaseQuery
{
    public List<TarefaDto>? Tarefas { get; set; }
    public required OperationResult OperationResult { get; set; }
}
