namespace Cpnucleo.Shared.Queries.ListApontamentoByRecurso;

public sealed record ListApontamentoByRecursoViewModel : BaseQuery
{
    public IEnumerable<ApontamentoDto> Apontamentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
