namespace Cpnucleo.Shared.Queries.GetTipoTarefa;

public sealed record GetTipoTarefaViewModel : BaseQuery
{
    public TipoTarefaDto TipoTarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
