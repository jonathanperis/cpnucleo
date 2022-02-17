namespace Cpnucleo.Infra.CrossCutting.Util.Queries.Projeto;

public class ListProjetoViewModel
{
    public IEnumerable<ProjetoDTO> Projetos { get; set; }
    public OperationResult OperationResult { get; set; }
}
