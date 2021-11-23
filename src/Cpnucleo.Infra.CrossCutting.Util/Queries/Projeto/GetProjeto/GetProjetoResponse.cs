namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.GetProjeto;

public class GetProjetoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public ProjetoViewModel Projeto { get; set; }
}