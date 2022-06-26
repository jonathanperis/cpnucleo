namespace Cpnucleo.Shared.Queries.Sistema;

public record GetSistemaViewModel : BaseQuery
{
    public SistemaDTO Sistema { get; set; }
    public OperationResult OperationResult { get; set; }
}
