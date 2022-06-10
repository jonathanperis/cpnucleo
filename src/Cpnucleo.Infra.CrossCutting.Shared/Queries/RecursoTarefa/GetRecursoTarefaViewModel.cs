namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.RecursoTarefa;

public class GetRecursoTarefaViewModel : BaseQuery
{
    public RecursoTarefaDTO RecursoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
