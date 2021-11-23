namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.ListRecursoTarefa;

public class ListRecursoTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<RecursoTarefaViewModel> RecursoTarefas { get; set; }
}