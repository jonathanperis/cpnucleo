namespace Cpnucleo.Shared.Queries.Recurso;

public sealed record GetRecursoViewModel : BaseQuery
{
    public RecursoDTO Recurso { get; set; }
    public OperationResult OperationResult { get; set; }
}
