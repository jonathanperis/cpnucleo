namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Apontamento;

public class GetApontamentoViewModel : BaseQuery
{
    public ApontamentoDTO Apontamento { get; set; }
    public OperationResult OperationResult { get; set; }
}
