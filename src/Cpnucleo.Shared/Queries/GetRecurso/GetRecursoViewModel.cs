namespace Cpnucleo.Shared.Queries.GetRecurso;

public sealed record GetRecursoViewModel : BaseQuery
{
    public RecursoDTO Recurso { get; set; }
    public OperationResult OperationResult { get; set; }
}
