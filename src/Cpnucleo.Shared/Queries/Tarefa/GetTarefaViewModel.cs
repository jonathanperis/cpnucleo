namespace Cpnucleo.Shared.Queries.Tarefa;

public sealed record GetTarefaViewModel : BaseQuery
{
    public TarefaDTO Tarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
