namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Apontamento;

public record GetApontamentoByRecursoViewModel : BaseQuery
{
    public IEnumerable<ApontamentoDTO> Apontamentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
