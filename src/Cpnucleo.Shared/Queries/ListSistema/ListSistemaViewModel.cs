namespace Cpnucleo.Shared.Queries.ListSistema;

public sealed record ListSistemaViewModel : BaseQuery
{
    public List<SistemaDto>? Sistemas { get; set; }
    public required OperationResult OperationResult { get; set; }
}
