namespace Cpnucleo.Shared.Queries.ListApontamentoByRecurso;

public sealed record ListApontamentoByRecursoViewModel : BaseQuery
{
    public List<ApontamentoDto>? Apontamentos { get; set; }
    public required OperationResult OperationResult { get; set; }
}
