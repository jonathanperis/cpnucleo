namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Projeto;

public class ListProjetoViewModel : BaseQuery
{
    public IEnumerable<ProjetoDTO> Projetos { get; set; }
    public OperationResult OperationResult { get; set; }
}
