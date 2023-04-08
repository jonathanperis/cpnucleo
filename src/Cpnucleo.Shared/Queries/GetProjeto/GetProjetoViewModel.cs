namespace Cpnucleo.Shared.Queries.GetProjeto;

public sealed record GetProjetoViewModel : BaseQuery
{
    public ProjetoDTO Projeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
