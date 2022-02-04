namespace Cpnucleo.Application.Queries.Tarefa.ListTarefa;

public class ListTarefaViewModel
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
