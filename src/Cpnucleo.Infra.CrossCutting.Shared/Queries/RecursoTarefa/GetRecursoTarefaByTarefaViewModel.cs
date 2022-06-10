namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public class GetRecursoTarefaByTarefaViewModel : BaseQuery
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
