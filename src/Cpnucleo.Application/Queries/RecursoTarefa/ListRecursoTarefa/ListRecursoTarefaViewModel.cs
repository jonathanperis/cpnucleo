namespace Cpnucleo.Application.Queries.RecursoTarefa.ListRecursoTarefa;

public class ListRecursoTarefaViewModel
{
    public IEnumerable<RecursoTarefaDTO> RecursoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
