namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Tarefa.GetByRecurso;

public class GetByRecursoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<TarefaViewModel> Tarefas { get; set; }
}