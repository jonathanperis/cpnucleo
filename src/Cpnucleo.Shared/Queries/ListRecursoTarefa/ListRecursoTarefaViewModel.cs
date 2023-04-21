namespace Cpnucleo.Shared.Queries.ListRecursoTarefa;

public sealed record ListRecursoTarefaViewModel : BaseQuery
{
    public List<RecursoTarefaDto>? RecursoTarefas { get; set; }
    public required OperationResult OperationResult { get; set; }
}
