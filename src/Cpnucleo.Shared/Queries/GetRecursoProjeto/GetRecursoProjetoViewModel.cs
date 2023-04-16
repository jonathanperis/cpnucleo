namespace Cpnucleo.Shared.Queries.GetRecursoProjeto;

public sealed record GetRecursoProjetoViewModel : BaseQuery
{
    public RecursoProjetoDto RecursoProjeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
