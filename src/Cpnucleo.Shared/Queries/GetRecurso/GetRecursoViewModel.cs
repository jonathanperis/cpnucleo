namespace Cpnucleo.Shared.Queries.GetRecurso;

public sealed record GetRecursoViewModel : BaseQuery
{
    public RecursoDto? Recurso { get; set; }
    public required OperationResult OperationResult { get; set; }
}
