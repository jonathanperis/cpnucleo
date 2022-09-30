namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public sealed record ListRecursoProjetoViewModel : BaseQuery
{
    public IEnumerable<RecursoProjetoDTO> RecursoProjetos { get; set; }
    public OperationResult OperationResult { get; set; }
}
