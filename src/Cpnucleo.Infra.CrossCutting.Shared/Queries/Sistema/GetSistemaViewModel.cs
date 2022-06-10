namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Sistema;

public class GetSistemaViewModel : BaseQuery
{
    public SistemaDTO Sistema { get; set; }
    public OperationResult OperationResult { get; set; }
}
