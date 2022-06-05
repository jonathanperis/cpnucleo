namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Recurso;

public class GetRecursoViewModel : BaseQuery
{
    public RecursoDTO Recurso { get; set; }
    public OperationResult OperationResult { get; set; }
}
