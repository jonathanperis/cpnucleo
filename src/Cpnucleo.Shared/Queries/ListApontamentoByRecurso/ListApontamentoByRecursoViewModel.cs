namespace Cpnucleo.Shared.Queries.ListApontamentoByRecurso;

public sealed record ListApontamentoByRecursoViewModel : BaseQuery
{
    public IEnumerable<ApontamentoDTO> Apontamentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
