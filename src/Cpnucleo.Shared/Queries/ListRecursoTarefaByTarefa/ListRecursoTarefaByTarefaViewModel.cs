namespace Cpnucleo.Shared.Queries.ListRecursoTarefaByTarefa;

public sealed record ListRecursoTarefaByTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
