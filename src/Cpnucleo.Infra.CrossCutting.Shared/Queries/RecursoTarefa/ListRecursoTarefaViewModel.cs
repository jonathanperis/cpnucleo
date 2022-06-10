namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public class ListRecursoTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
