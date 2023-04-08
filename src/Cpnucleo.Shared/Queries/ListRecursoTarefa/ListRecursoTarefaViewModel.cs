namespace Cpnucleo.Shared.Queries.ListRecursoTarefa;

public sealed record ListRecursoTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
