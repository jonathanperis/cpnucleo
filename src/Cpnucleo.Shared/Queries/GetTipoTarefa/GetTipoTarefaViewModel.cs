namespace Cpnucleo.Shared.Queries.GetTipoTarefa;

public sealed record GetTipoTarefaViewModel : BaseQuery
{
    public TipoTarefaDTO TipoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
