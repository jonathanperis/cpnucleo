namespace Cpnucleo.Shared.Queries.ListSistema;

public sealed record ListSistemaViewModel : BaseQuery
{
    public IEnumerable<SistemaDto>? Sistemas { get; set; }
    public OperationResult OperationResult { get; set; }
}
