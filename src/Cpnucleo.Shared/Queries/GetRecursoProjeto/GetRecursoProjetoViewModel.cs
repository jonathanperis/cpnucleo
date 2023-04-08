namespace Cpnucleo.Shared.Queries.GetRecursoProjeto;

public sealed record GetRecursoProjetoViewModel : BaseQuery
{
    public RecursoProjetoDTO RecursoProjeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
