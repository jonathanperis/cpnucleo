namespace Cpnucleo.Application.Queries.Apontamento.ListApontamento;

public class ListApontamentoViewModel
{
    public IEnumerable<ApontamentoDTO> Apontamentos { get; set; }
    public OperationResult OperationResult { get; set; }
}
