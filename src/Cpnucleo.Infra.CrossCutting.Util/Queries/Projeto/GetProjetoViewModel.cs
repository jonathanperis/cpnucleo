namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

public class GetProjetoViewModel : BaseQuery
{
    public ProjetoDTO Projeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
