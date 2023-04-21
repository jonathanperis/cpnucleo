namespace Cpnucleo.Shared.Queries.ListTipoTarefa;

public sealed record ListTipoTarefaViewModel : BaseQuery
{
    public List<TipoTarefaDto>? TipoTarefas { get; set; }
    public required OperationResult OperationResult { get; set; }
}
