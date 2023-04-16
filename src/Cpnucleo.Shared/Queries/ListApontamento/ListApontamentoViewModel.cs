namespace Cpnucleo.Shared.Queries.ListApontamento;

public sealed record ListApontamentoViewModel : BaseQuery
{
    public IEnumerable<ApontamentoDto> Apontamentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
