namespace Cpnucleo.Shared.Queries.Apontamento;

public sealed record GetApontamentoViewModel : BaseQuery
{
    public ApontamentoDTO Apontamento { get; set; }
    public OperationResult OperationResult { get; set; }
}
