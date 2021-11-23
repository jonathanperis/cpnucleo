namespace Cpnucleo.Infra.CrossCutting.Util.Queries.RecursoTarefa.GetRecursoTarefa;

public class GetRecursoTarefaResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public RecursoTarefaViewModel RecursoTarefa { get; set; }
}