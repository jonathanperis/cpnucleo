namespace Cpnucleo.Shared.Queries.GetRecursoProjeto;

public sealed record GetRecursoProjetoViewModel : BaseQuery
{
    public RecursoProjetoDto? RecursoProjeto { get; set; }
    public required OperationResult OperationResult { get; set; }
}
