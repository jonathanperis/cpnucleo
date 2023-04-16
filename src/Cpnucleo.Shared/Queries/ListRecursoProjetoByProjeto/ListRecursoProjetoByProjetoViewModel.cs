namespace Cpnucleo.Shared.Queries.ListRecursoProjetoByProjeto;

public sealed record ListRecursoProjetoByProjetoViewModel : BaseQuery
{
    public IEnumerable<RecursoProjetoDto> RecursoProjetos { get; set; }
    public OperationResult OperationResult { get; set; }
}
