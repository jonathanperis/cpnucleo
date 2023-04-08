namespace Cpnucleo.Shared.Queries.ListRecurso;

public sealed record ListRecursoViewModel : BaseQuery
{
    public IEnumerable<RecursoDTO> Recursos { get; set; }
    public OperationResult OperationResult { get; set; }
}
