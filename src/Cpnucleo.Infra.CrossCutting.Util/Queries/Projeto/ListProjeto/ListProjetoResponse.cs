namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto.ListProjeto;

public class ListProjetoResponse : BaseQuery
{
    public OperationResult Status { get; set; }

    public IEnumerable<ProjetoViewModel> Projetos { get; set; }
}