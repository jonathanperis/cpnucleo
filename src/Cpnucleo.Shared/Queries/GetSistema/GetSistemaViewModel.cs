namespace Cpnucleo.Shared.Queries.GetSistema;

public sealed record GetSistemaViewModel : BaseQuery
{
    public SistemaDto? Sistema { get; set; }
    public required OperationResult OperationResult { get; set; }
}
