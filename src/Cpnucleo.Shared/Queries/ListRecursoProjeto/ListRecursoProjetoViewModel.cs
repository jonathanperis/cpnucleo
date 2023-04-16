namespace Cpnucleo.Shared.Queries.ListRecursoProjeto;

public sealed record ListRecursoProjetoViewModel : BaseQuery
{
    public IEnumerable<RecursoProjetoDto> RecursoProjetos { get; set; }
    public OperationResult OperationResult { get; set; }
}
