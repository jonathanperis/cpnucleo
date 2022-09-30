namespace Cpnucleo.Shared.Queries.Recurso;

public sealed record ListRecursoViewModel : BaseQuery
{
    public IEnumerable<RecursoDTO> Recursos { get; set; }
    public OperationResult OperationResult { get; set; }
}
