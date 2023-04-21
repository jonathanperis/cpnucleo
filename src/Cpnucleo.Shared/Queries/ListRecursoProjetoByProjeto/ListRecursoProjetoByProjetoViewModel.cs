namespace Cpnucleo.Shared.Queries.ListRecursoProjetoByProjeto;

public sealed record ListRecursoProjetoByProjetoViewModel : BaseQuery
{
    public List<RecursoProjetoDto>? RecursoProjetos { get; set; }
    public required OperationResult OperationResult { get; set; }
}
