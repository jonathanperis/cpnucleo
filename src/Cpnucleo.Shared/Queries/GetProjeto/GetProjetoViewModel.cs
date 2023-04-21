namespace Cpnucleo.Shared.Queries.GetProjeto;

public sealed record GetProjetoViewModel : BaseQuery
{
    public ProjetoDto? Projeto { get; set; }
    public required OperationResult OperationResult { get; set; }
}
