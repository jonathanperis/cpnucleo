namespace Cpnucleo.Shared.Queries.ListRecursoProjeto;

public sealed record ListRecursoProjetoViewModel : BaseQuery
{
    public List<RecursoProjetoDto>? RecursoProjetos { get; set; }
    public required OperationResult OperationResult { get; set; }
}
