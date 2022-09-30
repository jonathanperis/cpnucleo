namespace Cpnucleo.Shared.Queries.RecursoProjeto;

public sealed record GetRecursoProjetoViewModel : BaseQuery
{
    public RecursoProjetoDTO RecursoProjeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
