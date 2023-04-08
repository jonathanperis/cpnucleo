namespace Cpnucleo.Shared.Queries.ListSistema;

public sealed record ListSistemaViewModel : BaseQuery
{
    public IEnumerable<SistemaDTO>? Sistemas { get; set; }
    public OperationResult OperationResult { get; set; }
}
