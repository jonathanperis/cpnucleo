namespace Cpnucleo.Shared.Queries.GetTipoTarefa;

public sealed record GetTipoTarefaViewModel : BaseQuery
{
    public TipoTarefaDto? TipoTarefa { get; set; }
    public required OperationResult OperationResult { get; set; }
}
