namespace Cpnucleo.Shared.Queries.GetSistema;

public sealed record GetSistemaViewModel : BaseQuery
{
    public SistemaDTO? Sistema { get; set; }
    public OperationResult OperationResult { get; set; }
}
