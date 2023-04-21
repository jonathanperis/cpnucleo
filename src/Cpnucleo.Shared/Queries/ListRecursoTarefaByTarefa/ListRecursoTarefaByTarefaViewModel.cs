namespace Cpnucleo.Shared.Queries.ListRecursoTarefaByTarefa;

public sealed record ListRecursoTarefaByTarefaViewModel : BaseQuery
{
    public List<RecursoTarefaDto>? RecursoTarefas { get; set; }
    public required OperationResult OperationResult { get; set; }
}
