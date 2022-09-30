namespace Cpnucleo.Shared.Queries.TipoTarefa;

public sealed record GetTipoTarefaViewModel : BaseQuery
{
    public TipoTarefaDTO TipoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
