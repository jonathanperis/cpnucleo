namespace Cpnucleo.Shared.Queries.Projeto;

public sealed record GetProjetoViewModel : BaseQuery
{
    public ProjetoDTO Projeto { get; set; }
    public OperationResult OperationResult { get; set; }
}
