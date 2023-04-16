namespace Cpnucleo.Shared.Queries.ListRecursoTarefaByTarefa;

public sealed record ListRecursoTarefaByTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDto> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
