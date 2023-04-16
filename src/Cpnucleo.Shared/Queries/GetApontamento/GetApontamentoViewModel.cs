namespace Cpnucleo.Shared.Queries.GetApontamento;

public sealed record GetApontamentoViewModel : BaseQuery
{
    public ApontamentoDto Apontamento { get; set; }
    public OperationResult OperationResult { get; set; }
}
