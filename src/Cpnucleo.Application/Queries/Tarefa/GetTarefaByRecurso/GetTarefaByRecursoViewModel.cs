namespace Cpnucleo.Application.Queries.Tarefa.GetByRecurso;

public class GetTarefaByRecursoViewModel
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
