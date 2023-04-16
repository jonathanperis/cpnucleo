namespace Cpnucleo.Shared.Queries.ListTipoTarefa;

public sealed record ListTipoTarefaViewModel : BaseQuery
{
    public IEnumerable<TipoTarefaDto> TipoTarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
