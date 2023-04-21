namespace Cpnucleo.Shared.Queries.GetTarefa;

public sealed record GetTarefaViewModel : BaseQuery
{
    public TarefaDto? Tarefa { get; set; }
    public required OperationResult OperationResult { get; set; }
}
