namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Recurso;

public record ListRecursoViewModel : BaseQuery
{
    public IEnumerable<RecursoDTO> Recursos { get; set; }
    public OperationResult OperationResult { get; set; }
}
