namespace Cpnucleo.Infra.CrossCutting.Shared.Queries.Projeto;

public record ListProjetoViewModel : BaseQuery
{
    public IEnumerable<ProjetoDTO> Projetos { get; set; }
    public OperationResult OperationResult { get; set; }
}
