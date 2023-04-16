namespace Cpnucleo.Shared.Queries.ListTarefa;

public sealed record ListTarefaViewModel : BaseQuery
{
    public IEnumerable<TarefaDto> Tarefas { get; set; }
    public OperationResult OperationResult { get; set; }
}
