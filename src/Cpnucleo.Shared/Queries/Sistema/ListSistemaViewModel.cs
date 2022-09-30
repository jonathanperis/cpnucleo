namespace Cpnucleo.Shared.Queries.Sistema;

public sealed record ListSistemaViewModel : BaseQuery
{
    public IEnumerable<SistemaDTO> Sistemas { get; set; }
    public OperationResult OperationResult { get; set; }
}
