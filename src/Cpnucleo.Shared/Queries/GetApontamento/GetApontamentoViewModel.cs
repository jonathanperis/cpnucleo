namespace Cpnucleo.Shared.Queries.GetApontamento;

public sealed record GetApontamentoViewModel : BaseQuery
{
    public ApontamentoDto? Apontamento { get; set; }
    public required OperationResult OperationResult { get; set; }
}
