namespace Cpnucleo.Shared.Queries.Recurso;

public record GetRecursoViewModel : BaseQuery
{
    public RecursoDTO Recurso { get; set; }
    public OperationResult OperationResult { get; set; }
}
