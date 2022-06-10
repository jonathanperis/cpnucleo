namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Tarefa;

public class GetTarefaByRecursoViewModel : BaseQuery
{
    public IEnumerable<TarefaDTO> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
