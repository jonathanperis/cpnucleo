namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa;

public class GetRecursoTarefaByTarefaViewModel
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
