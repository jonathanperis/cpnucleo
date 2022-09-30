namespace Cpnucleo.Shared.Queries.Sistema;

public sealed record GetSistemaViewModel : BaseQuery
{
    public SistemaDTO Sistema { get; set; }
    public OperationResult OperationResult { get; set; }
}
