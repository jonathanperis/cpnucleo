namespace Cpnucleo.Shared.Queries.ListTarefa;

public sealed record ListTarefaViewModel : BaseQuery
{
    public List<TarefaDto>? Tarefas { get; set; }
    public required OperationResult OperationResult { get; set; }
}
