namespace Cpnucleo.Shared.Queries.ListApontamento;

public sealed record ListApontamentoViewModel : BaseQuery
{
    public List<ApontamentoDto>? Apontamentos { get; set; }
    public required OperationResult OperationResult { get; set; }
}
