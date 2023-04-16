namespace Cpnucleo.Shared.Queries.ListTarefaByRecurso;

public sealed record ListTarefaByRecursoViewModel : BaseQuery
{
    public IEnumerable<TarefaDto> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
