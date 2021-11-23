namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetByTarefa;

public class GetByTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<RecursoTarefaViewModel> RecursoTarefas { get; set; }
}