namespace Cpnucleo.Shared.Queries.RecursoTarefa;

public record ListRecursoTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
