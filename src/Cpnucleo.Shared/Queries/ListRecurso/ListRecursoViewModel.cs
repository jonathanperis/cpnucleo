namespace Cpnucleo.Shared.Queries.ListRecurso;

public sealed record ListRecursoViewModel : BaseQuery
{
    public List<RecursoDto>? Recursos { get; set; }
    public required OperationResult OperationResult { get; set; }
}
