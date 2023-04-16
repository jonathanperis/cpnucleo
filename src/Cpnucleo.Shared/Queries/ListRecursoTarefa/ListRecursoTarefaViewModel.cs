namespace Cpnucleo.Shared.Queries.ListRecursoTarefa;

public sealed record ListRecursoTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDto> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
