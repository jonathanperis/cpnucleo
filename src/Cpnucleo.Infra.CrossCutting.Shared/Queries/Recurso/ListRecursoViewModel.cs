namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Recurso;

public class ListRecursoViewModel : BaseQuery
{
    public IEnumerable<RecursoDTO> Recursos { get; set; }
    public OperationResult OperationResult { get; set; }
}
