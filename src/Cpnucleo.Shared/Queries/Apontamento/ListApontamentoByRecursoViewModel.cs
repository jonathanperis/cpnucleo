namespace Cpnucleo.Shared.Queries.Apontamento;

public sealed record ListApontamentoByRecursoViewModel : BaseQuery
{
    public IEnumerable<ApontamentoDTO> Apontamentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
