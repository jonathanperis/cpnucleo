namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public record GetRecursoTarefaByTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
