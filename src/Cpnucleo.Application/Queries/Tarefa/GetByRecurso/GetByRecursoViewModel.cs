namespace Cpnucleo.Application.Queries.Tarefa.GetByRecurso;

public class GetByRecursoViewModel
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
