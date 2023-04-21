namespace Cpnucleo.Shared.Queries.ListProjeto;

public sealed record ListProjetoViewModel : BaseQuery
{
    public List<ProjetoDto>? Projetos { get; set; }
    public required OperationResult OperationResult { get; set; }
}
