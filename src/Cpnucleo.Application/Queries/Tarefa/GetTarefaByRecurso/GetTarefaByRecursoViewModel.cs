namespace Cpnucleo.Application.Queries.Tarefa.GetTarefaByRecurso;

public class GetTarefaByRecursoViewModel
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
