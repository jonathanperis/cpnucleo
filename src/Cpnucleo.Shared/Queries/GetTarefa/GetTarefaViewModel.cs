namespace Cpnucleo.Shared.Queries.GetTarefa;

public sealed record GetTarefaViewModel : BaseQuery
{
    public TarefaDto Tarefa { get; set; }
    public OperationResult OperationResult { get; set; }
}
